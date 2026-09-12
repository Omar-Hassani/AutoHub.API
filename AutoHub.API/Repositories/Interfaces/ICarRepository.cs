using AutoHub.API.Models;

namespace AutoHub.API.Repositories.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        void Update(Car car);
    }
}