using Booking.Domain.Models;

namespace Booking.DAL.Interfaces
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<City> GetByName(string name);
    }
}
