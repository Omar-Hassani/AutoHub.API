namespace AutoHub.API.Models
{
    public class CarImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // الربط مع جدول السيارات
        public int CarId { get; set; }
        public Car? Car { get; set; }
    }
}