namespace AutoHub.API.Models.Common
{
    public class ValidationErrorResponse
    {
        // 👈 تم تصحيح السطر وإزالة الكلمة الخاطئة
        public int StatusCode { get; set; } = 400;
        public string Title { get; set; } = "Validation Failed";
        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}