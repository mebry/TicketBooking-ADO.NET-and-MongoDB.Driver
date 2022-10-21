using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;

namespace Booking.Service.Implementations
{
    public class TripTreatmentService: ITripTreatmentService
    {
        public TripTreatmentService() { }

        public BaseResponse<List<int>> GetAvailablePlaces(List<TripDetails> tripDetail,int capacity)
        {
            List<int> bookedPlaces = new List<int>();
            List<int> availablePlaces = new List<int>();
            foreach (var item in tripDetail)
            {
                bookedPlaces.Add(item.Place);
            }
            
            for (int i = 1; i <= capacity; i++)
            {
                if (bookedPlaces.Contains(i))
                {
                    continue;
                }

                availablePlaces.Add(i);

            }

            return new BaseResponse<List<int>>()
            {
                Data = availablePlaces,
                StatusCode = StatusCode.OK,
                Description = "Places were found"
            };

        }
    }
}
