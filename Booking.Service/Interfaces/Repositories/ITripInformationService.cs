using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;
using Booking.Domain.ViewModels.Trip;

namespace Booking.Service.Interfaces.Repositories
{
    public interface ITripInformationService
    {
        Task<BaseResponse<List<UserTripViewModel>>> GetTripsByUserId(int userId);
        Task<BaseResponse<List<UserTripViewModel>>> GetPlannedTripsByUser(int userId);
        Task<BaseResponse<List<UserTripViewModel>>> GetExecutedTripsByUser(int userId);
        Task<BaseResponse<List<TripInfo>>> GetInformationByTrips();
        Task<BaseResponse<List<TripInfo>>> GetInformationByTripsInside();
        Task<BaseResponse<List<TripInfo>>> GetInformationByTripsOutside();
        Task<BaseResponse<List<TripInfo>>> GetInformationByDate(int countDays);
        Task<BaseResponse<TripInfo>> GetInformationByTrip(int tripId);
    }
}
