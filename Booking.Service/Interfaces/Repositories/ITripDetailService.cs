using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Repositories
{
    public interface ITripDetailService : IBaseService<TripDetails>
    {
        Task<BaseResponse<bool>> DeleteByPlace(int tripId, int place);
        Task<BaseResponse<List<TripDetails>>> GetDetailsByTripId(int tripId);
    }
}
