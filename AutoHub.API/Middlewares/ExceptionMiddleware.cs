// Middlewares/ExceptionMiddleware.cs
using System.Net;
using System.Text.Json;
using AutoHub.API.Models.Common;

namespace AutoHub.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // في بيئة التطوير (Development) نظهر تفاصيل الخطأ لتسهيل ה-Debugging
            // وفي بيئة الإنتاج (Production) نكتفي برسالة آمنة عامة
            var message = _env.IsDevelopment() ? exception.Message : "حدث خطأ غير متوقع في السيرفر. يرجى المحاولة لاحقاً.";
            var errors = _env.IsDevelopment() ? new List<string> { exception.StackTrace ?? string.Empty } : null;

            var response = ApiResponse<object>.Failure(message, errors);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(json);
        }
    }
}