using Booking.Domain.Models;
using Booking.Domain.ViewModels.Trip;

namespace Booking.DAL.Interfaces
{
    public interface ITripRepository : IBaseRepository<Trip>
    {
        Task<List<UserTripViewModel>> GetTripsByUserId(int id);
    }
}
