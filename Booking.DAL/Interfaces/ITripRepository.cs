using Booking.Domain.Models;

namespace Booking.DAL.Interfaces
{
    public interface ITripRepository : IBaseRepository<Trip>
    {
        /*Task<City> GetByName(string name);*/
    }
}
