using AutoHub.API.Data;
using AutoHub.API.Models;
using AutoHub.API.Repositories.Interfaces;

namespace AutoHub.API.Repositories.Implementations
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext db) : base(db) { }

        public void Update(Car car)
        {
            _db.Cars.Update(car);
        }
    }
}