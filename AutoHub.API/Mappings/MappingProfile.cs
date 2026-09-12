using AutoMapper;
using AutoHub.API.DTOs;
using AutoHub.API.Models;
using Microsoft.AspNetCore.Http;

namespace AutoHub.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // تحويل CarImage إلى CarImageDto مع تحويل المسار النسبي إلى URL كامل
            CreateMap<CarImage, CarImageDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<FullUrlResolver>());

            CreateMap<Car, CarResponseDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.CarImages));

            CreateMap<CarCreateUpdateDto, Car>();
        }
    }

    // Resolver لبناء الرابط الكامل تلقائياً
    public class FullUrlResolver : IValueResolver<CarImage, CarImageDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FullUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(CarImage source, CarImageDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ImageUrl))
                return string.Empty;

            // إذا كان الرابط مكتمل مسبقاً (مثلاً رابط خارجي)
            if (source.ImageUrl.StartsWith("http://") || source.ImageUrl.StartsWith("https://"))
                return source.ImageUrl;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return source.ImageUrl;

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}{source.ImageUrl}";
        }
    }
}