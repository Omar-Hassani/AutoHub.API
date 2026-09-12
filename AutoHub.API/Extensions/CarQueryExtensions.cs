using System.Linq;
using AutoHub.API.DTOs;
using AutoHub.API.Models;

namespace AutoHub.API.Extensions
{
    public static class CarQueryExtensions
    {
        // 1. الفلترة حسب الخصائص
        public static IQueryable<Car> FilterCars(this IQueryable<Car> query, CarQueryParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.Make))
                query = query.Where(c => c.Make.ToLower().Contains(parameters.Make.ToLower().Trim()));

            if (!string.IsNullOrWhiteSpace(parameters.Model))
                query = query.Where(c => c.Model.ToLower().Contains(parameters.Model.ToLower().Trim()));

            if (parameters.MinPrice.HasValue)
                query = query.Where(c => c.Price >= parameters.MinPrice.Value);

            if (parameters.MaxPrice.HasValue)
                query = query.Where(c => c.Price <= parameters.MaxPrice.Value);

            if (parameters.MinYear.HasValue)
                query = query.Where(c => c.Year >= parameters.MinYear.Value);

            if (parameters.MaxYear.HasValue)
                query = query.Where(c => c.Year <= parameters.MaxYear.Value);

            return query;
        }

        // 2. البحث النصي الشامل
        public static IQueryable<Car> SearchCars(this IQueryable<Car> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return query;

            var term = searchTerm.Trim().ToLower();

            return query.Where(c =>
                c.Make.ToLower().Contains(term) ||
                c.Model.ToLower().Contains(term) ||
                (c.Description != null && c.Description.ToLower().Contains(term)));
        }

        // 3. الترتيب (Sorting)
        public static IQueryable<Car> SortCars(this IQueryable<Car> query, string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(c => c.Price),
                "price_desc" => query.OrderByDescending(c => c.Price),
                "oldest" => query.OrderBy(c => c.Id), // 👈 استخدام Id بدلاً من CreatedAt
                "year_desc" => query.OrderByDescending(c => c.Year),
                "year_asc" => query.OrderBy(c => c.Year),
                _ => query.OrderByDescending(c => c.Id) // 👈 الافتراضي: الأحدث إدخالاً بواسطة Id
            };
        }
    }
}