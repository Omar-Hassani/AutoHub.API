using AutoHub.API.Data;
using AutoHub.API.Models;
using AutoHub.API.Repositories.Interfaces;

namespace AutoHub.API.Repositories.Implementations
{
    public class SalesRequestRepository : Repository<SalesRequest>, ISalesRequestRepository
    {
        public SalesRequestRepository(ApplicationDbContext db) : base(db) { }

        public void Update(SalesRequest salesRequest)
        {
            _db.SalesRequests.Update(salesRequest);
        }
    }
}