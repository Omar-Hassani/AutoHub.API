using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoHub.API.DTOs;
using AutoHub.API.Extensions;
using AutoHub.API.Helpers;
using AutoHub.API.Models;
using AutoHub.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 1. جلب جميع السيارات مع الفلترة والبحث والترتيب والصفحات (متاح للجميع)
        [HttpGet]
        public async Task<IActionResult> GetCars([FromQuery] CarQueryParameters queryParams)
        {
            // إعداد الـ Query الشامل للصور
            var query = _unitOfWork.Cars.GetQueryable(includeProperties: "CarImages");

            // تطبيق الفلترة، البحث، والترتيب
            query = query.FilterCars(queryParams)
                         .SearchCars(queryParams.SearchTerm)
                         .SortCars(queryParams.SortBy);

            // حساب الإجمالي للـ Pagination
            int totalCount = await query.CountAsync();

            // تطبيق Pagination على قاعدة البيانات مباشرة
            var cars = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            // تحويل النتائج للـ Response DTO
            var carDtos = _mapper.Map<IEnumerable<CarResponseDto>>(cars);

            var result = new PagedResult<CarResponseDto>(
                carDtos,
                totalCount,
                queryParams.PageNumber,
                queryParams.PageSize
            );

            return Ok(result);
        }

        // 2. جلب سيارة محددة بواسطة الـ Id (متاح للجميع)
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _unitOfWork.Cars.GetAsync(c => c.Id == id, includeProperties: "CarImages");
            if (car == null)
                return NotFound($"السيارة ذات الرقم {id} غير موجودة.");

            var carDto = _mapper.Map<CarResponseDto>(car);

            return Ok(carDto);
        }

        // 3. إضافة سيارة جديدة (فقط للـ Admin والـ Dealer)
        [HttpPost]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> CreateCar([FromBody] CarCreateUpdateDto carDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var car = _mapper.Map<Car>(carDto);
            car.IsAvailable = true;

            await _unitOfWork.Cars.AddAsync(car);
            await _unitOfWork.SaveAsync();

            var carResponse = _mapper.Map<CarResponseDto>(car);

            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, carResponse);
        }

        // 4. تعديل بيانات سيارة (فقط للـ Admin والـ Dealer)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarCreateUpdateDto carDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var carFromDb = await _unitOfWork.Cars.GetAsync(c => c.Id == id);
            if (carFromDb == null)
                return NotFound($"السيارة ذات الرقم {id} غير موجودة.");

            _mapper.Map(carDto, carFromDb);

            _unitOfWork.Cars.Update(carFromDb);
            await _unitOfWork.SaveAsync();

            return Ok("تم تحديث بيانات السيارة بنجاح.");
        }

        // 5. حذف سيارة (فقط للـ Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _unitOfWork.Cars.GetAsync(c => c.Id == id);
            if (car == null)
                return NotFound($"السيارة ذات الرقم {id} غير موجودة.");

            _unitOfWork.Cars.Remove(car);
            await _unitOfWork.SaveAsync();

            return Ok("تم حذف السيارة بنجاح.");
        }
    }
}