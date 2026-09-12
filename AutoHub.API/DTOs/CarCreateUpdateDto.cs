using System.ComponentModel.DataAnnotations;
using AutoHub.API.Models;

namespace AutoHub.API.DTOs
{
    public class CarCreateUpdateDto
    {
        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public CarCondition Condition { get; set; } // ForSale أو ForRent

        public string FuelType { get; set; } = string.Empty;

        public int Mileage { get; set; }

        public string Description { get; set; } = string.Empty;


    }
}