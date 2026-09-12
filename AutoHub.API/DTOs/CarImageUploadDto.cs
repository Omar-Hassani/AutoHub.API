using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoHub.API.DTOs
{
    public class CarImageUploadDto
    {
        [Required(ErrorMessage = "يرجى تحديد الصورة المراد رفعها")]
        public IFormFile Image { get; set; } = null!;

        public bool IsMain { get; set; } = false;
    }
}