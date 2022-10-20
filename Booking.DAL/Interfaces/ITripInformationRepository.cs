using Booking.Domain.ViewModels.Trip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Interfaces
{
    public interface ITripInformationRepository
    {
        Task<List<UserTripViewModel>> GetTripsByUserId(int id);
        Task<List<UserTripViewModel>> GetPlannedTripsByUser(int userId);
        Task<List<UserTripViewModel>> GetExecutedTripsByUser(int userId);
        Task<List<TripInfo>> GetInformationByTrips();
        Task<List<TripInfo>> GetInformationByTripsInside();
        Task<List<TripInfo>> GetInformationByTripsOutside();
        Task<List<TripInfo>> GetInformationByDate(int countDays);
        Task<TripInfo> GetInformationByTrip(int tripId);
    }
}
