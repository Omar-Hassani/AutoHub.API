using System;

namespace AutoHub.API.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }

        public string CustomerId { get; set; } = string.Empty; // سنربطه لاحقاً بجدول المستخدمين

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Active, Completed, Cancelled
    }
}