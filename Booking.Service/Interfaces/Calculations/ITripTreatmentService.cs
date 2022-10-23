using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Calculations
{
    public interface ITripTreatmentService
    {
        BaseResponse<List<int>> GetAvailablePlaces(List<TripDetails> tripDetails,int capacity);
    }
}
