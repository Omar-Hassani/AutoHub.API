using System.Threading.Tasks;
using AutoHub.API.Data;
using AutoHub.API.Repositories.Interfaces;

namespace AutoHub.API.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICarRepository Cars { get; private set; }
        public IRentalRepository Rentals { get; private set; }
        public ISalesRequestRepository SalesRequests { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Cars = new CarRepository(_db);
            Rentals = new RentalRepository(_db);
            SalesRequests = new SalesRequestRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}