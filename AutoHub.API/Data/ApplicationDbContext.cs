using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoHub.API.Models;

namespace AutoHub.API.Data
{
    // ورثنا من IdentityDbContext لكي يدعم النظام جداول المستخدمين والصلاحيات (Identity) تلقائياً
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // تسجيل الجداول في قاعدة البيانات
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<SalesRequest> SalesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تحديد العلاقات والقيود بين الجداول (Fluent API)

            // 1. علاقة السيارة مع الصور (One-to-Many)
            modelBuilder.Entity<CarImage>()
                .HasOne(ci => ci.Car)
                .WithMany(c => c.CarImages)
                .HasForeignKey(ci => ci.CarId)
                .OnDelete(DeleteBehavior.Cascade); // إذا حذفت السيارة تحذف صورها تلقائياً

            // 2. علاقة السيارة مع طلبات الإيجار (One-to-Many)
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict); // نمنع حذف السيارة إذا كانت مرتبطة بعقد إيجار

            // 3. علاقة السيارة مع طلبات الشراء (One-to-Many)
            modelBuilder.Entity<SalesRequest>()
                .HasOne(sr => sr.Car)
                .WithMany(c => c.SalesRequests)
                .HasForeignKey(sr => sr.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            // إعدادات إضافية للدقة المالية (السعر الإجمالي وسعر السيارة)
            modelBuilder.Entity<Car>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}