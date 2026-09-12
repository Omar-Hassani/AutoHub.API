using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoHub.API.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AutoHub.API.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 Megabytes

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<(bool IsSuccess, string Message, string FilePath)> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return (false, "لم يتم اختيار أي ملف.", string.Empty);

            // 1. التحقق من حجم الملف
            if (file.Length > MaxFileSizeInBytes)
                return (false, "حجم الصورة يتجاوز الحد الأقصى المسموح (5 ميجابايت).", string.Empty);

            // 2. التحقق من امتداد الملف
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return (false, "نوع الملف غير مدعوم! الصيغ المسموحة هي: JPG, JPEG, PNG, WEBP.", string.Empty);

            // 3. إنتاج مسار المجلد داخل wwwroot
            var wwwrootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolderPath = Path.Combine(wwwrootPath, folderName);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            // 4. توليد اسم فريد للملف لمنع التضارب
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolderPath, uniqueFileName);

            // 5. حفظ الملف على القرص
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // إرجاع المسار النسبي (Relative Path)
            var relativePath = $"/{folderName}/{uniqueFileName}";
            return (true, string.Empty, relativePath);
        }

        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return false;

            var wwwrootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(wwwrootPath, relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}