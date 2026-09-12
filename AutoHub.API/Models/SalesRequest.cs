using System;

namespace AutoHub.API.Models
{
    public class SalesRequest
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }

        public string CustomerId { get; set; } = string.Empty; // سنربطه لاحقاً بجدول المستخدمين

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}