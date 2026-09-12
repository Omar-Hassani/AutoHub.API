using AutoHub.API.Models;

namespace AutoHub.API.Repositories.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        void Update(Rental rental);
    }
}