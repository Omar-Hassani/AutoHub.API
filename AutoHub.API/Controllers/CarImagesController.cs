using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoHub.API.DTOs;
using AutoHub.API.Models;
using AutoHub.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public CarImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment environment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
            _mapper = mapper;
        }

        // 1. رفع صورة أو عدة صور لسيارة معينة
        [HttpPost("upload/{carId:int}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> UploadImages(int carId, [FromForm] IFormFileCollection files)
        {
            var car = await _unitOfWork.Cars.GetAsync(c => c.Id == carId, includeProperties: "CarImages");
            if (car == null)
                return NotFound($"السيارة ذات الرقم {carId} غير موجودة.");

            if (files == null || files.Count == 0)
                return BadRequest("لم يتم اختيار أي صور للرفع.");

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    return BadRequest($"الملف '{file.FileName}' نوعه غير مدعوم. الصيغ المسموحة هي: JPG, JPEG, PNG, WEBP.");

                if (file.Length > MaxFileSizeInBytes)
                    return BadRequest($"الملف '{file.FileName}' يتجاوز الحد الأقصى للحجم (5 ميجابايت).");
            }

            string wwwRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(wwwRootPath, "images", "cars");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uploadedImages = new List<CarImage>();

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                string relativePath = $"/images/cars/{fileName}";

                var carImage = new CarImage
                {
                    CarId = carId,
                    ImageUrl = relativePath
                };

                car.CarImages.Add(carImage);
                uploadedImages.Add(carImage);
            }

            await _unitOfWork.SaveAsync();

            var resultDto = _mapper.Map<IEnumerable<CarImageDto>>(uploadedImages);

            return Ok(resultDto);
        }

        // 2. تعديل/استبدال صورة محددة بواسطة imageId
        [HttpPut("{imageId:int}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> UpdateImage(int imageId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("يرجى اختيار صورة جديدة للاستبدال.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return BadRequest("نوع الملف غير مدعوم. الصيغ المسموحة هي: JPG, JPEG, PNG, WEBP.");

            if (file.Length > MaxFileSizeInBytes)
                return BadRequest("حجم الصورة يتجاوز الحد الأقصى المسموح (5 ميجابايت).");

            // البحث عن السيارة التي تحتوي على الصورة
            var cars = await _unitOfWork.Cars.GetAllAsync(includeProperties: "CarImages");
            var parentCar = cars.FirstOrDefault(c => c.CarImages.Any(img => img.Id == imageId));

            if (parentCar == null)
                return NotFound($"الصورة ذات الرقم {imageId} غير موجودة.");

            var targetImage = parentCar.CarImages.First(img => img.Id == imageId);

            string wwwRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(wwwRootPath, "images", "cars");

            // 1. حذف الصورة القديمة من القرص
            string oldFileName = Path.GetFileName(targetImage.ImageUrl);
            string oldFilePath = Path.Combine(uploadsFolder, oldFileName);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            // 2. حفظ الصورة الجديدة
            string newFileName = Guid.NewGuid().ToString() + extension;
            string newFilePath = Path.Combine(uploadsFolder, newFileName);

            using (var fileStream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 3. تحديث المسار النسبي للصورة
            targetImage.ImageUrl = $"/images/cars/{newFileName}";

            await _unitOfWork.SaveAsync();

            var imageDto = _mapper.Map<CarImageDto>(targetImage);

            return Ok(imageDto);
        }

        // 3. حذف صورة محددة بواسطة Id الصورة
        [HttpDelete("{imageId:int}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var cars = await _unitOfWork.Cars.GetAllAsync(includeProperties: "CarImages");
            var parentCar = cars.FirstOrDefault(c => c.CarImages.Any(img => img.Id == imageId));

            if (parentCar == null)
                return NotFound($"الصورة ذات الرقم {imageId} غير موجودة.");

            var targetImage = parentCar.CarImages.First(img => img.Id == imageId);

            // حذف الملف الفعلي من القرص الصلب
            string wwwRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string fileName = Path.GetFileName(targetImage.ImageUrl);
            string filePath = Path.Combine(wwwRootPath, "images", "cars", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // حذف سجل الصورة من القائمة
            parentCar.CarImages.Remove(targetImage);
            await _unitOfWork.SaveAsync();

            return Ok("تم حذف الصورة بنجاح.");
        }
    }
}