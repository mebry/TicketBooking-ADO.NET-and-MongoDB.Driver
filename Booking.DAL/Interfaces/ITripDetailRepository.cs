using Booking.Domain.Models;


namespace Booking.DAL.Interfaces
{
    public interface ITripDetailRepository:IBaseRepository<TripDetails>
    {
        Task<bool> DeleteByPlace(int tripId, int place);
        Task<List<TripDetails>> GetDetailsByTripId(int tripId);
    }
}
