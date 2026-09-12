using System;
using System.Collections.Generic;

namespace AutoHub.API.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public CarCondition Condition { get; set; } // ForSale أو ForRent
        public bool IsAvailable { get; set; } = true;
        public string FuelType { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public string Description { get; set; } = string.Empty;

        // علاقات Entity Framework
        public ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<SalesRequest> SalesRequests { get; set; } = new List<SalesRequest>();
    }
}