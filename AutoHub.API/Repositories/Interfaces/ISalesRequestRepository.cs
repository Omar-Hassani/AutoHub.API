using AutoHub.API.Models;

namespace AutoHub.API.Repositories.Interfaces
{
    public interface ISalesRequestRepository : IRepository<SalesRequest>
    {
        void Update(SalesRequest salesRequest);
    }
}