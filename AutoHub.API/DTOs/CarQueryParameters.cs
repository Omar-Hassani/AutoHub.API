namespace AutoHub.API.DTOs
{
    public class CarQueryParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        // Pagination
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Filtering
        public string? Make { get; set; }
        public string? Model { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }

        // Searching (بحث عام في الشركة، الموديل، أو الوصف)
        public string? SearchTerm { get; set; }

        // Sorting (مثل: "price_asc", "price_desc", "newest", "oldest")
        public string? SortBy { get; set; } = "newest";
    }
}