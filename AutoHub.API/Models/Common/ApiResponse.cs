// Models/Common/ApiResponse.cs
namespace AutoHub.API.Models.Common
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Success(T data, string message = "")
            => new() { IsSuccess = true, Data = data, Message = message };

        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
            => new() { IsSuccess = false, Message = message, Errors = errors };
    }
}