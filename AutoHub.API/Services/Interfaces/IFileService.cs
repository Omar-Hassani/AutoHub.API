using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutoHub.API.Services.Interfaces
{
    public interface IFileService
    {
        Task<(bool IsSuccess, string Message, string FilePath)> SaveFileAsync(IFormFile file, string allowedFoldersPath);
        bool DeleteFile(string filePath);
    }
}