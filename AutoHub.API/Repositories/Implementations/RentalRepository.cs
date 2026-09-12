using AutoHub.API.Data;
using AutoHub.API.Models;
using AutoHub.API.Repositories.Interfaces;

namespace AutoHub.API.Repositories.Implementations
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext db) : base(db) { }

        public void Update(Rental rental)
        {
            _db.Rentals.Update(rental);
        }
    }
}