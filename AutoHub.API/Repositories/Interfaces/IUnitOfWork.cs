using System;
using System.Threading.Tasks;

namespace AutoHub.API.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICarRepository Cars { get; }
        IRentalRepository Rentals { get; }
        ISalesRequestRepository SalesRequests { get; }
        Task SaveAsync();
    }
}