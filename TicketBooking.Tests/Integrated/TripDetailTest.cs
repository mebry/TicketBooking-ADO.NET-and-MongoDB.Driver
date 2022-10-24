using Xunit;
using Moq;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;
using Booking.Service.Implementations;
using Booking.Service.Interfaces.Repositories;
using Booking.Service.Implementations.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using Booking.Domain.Enums;
using System.Linq;
using Booking.Service.Interfaces.Calculations;
using Booking.Service.Implementations.Calculations;

namespace TicketBooking.Tests.Integrated
{
    public class TripDetailTest
    {
        [Fact]
        public async Task GetTripDetailsAndFindAvailablePlaces_IsTrue()
        {
            // Arrange
            int tripId = 1;
            int capacity = 10;
            var details = GetTripDetails();
            var count = details.Count;

            var tripDetailMock = new Mock<ITripDetailRepository>();
            tripDetailMock.Setup(repo => repo.GetDetailsByTripId(tripId))
                .ReturnsAsync(details);

            var tripMock = new Mock<ITripRepository>();
            tripMock.Setup(repo => repo.GetById(tripId))
                .ReturnsAsync(new Trip());

            ITripDetailService tripDetailService = new TripDetailService(tripDetailMock.Object, tripMock.Object, null);
            ITripTreatmentService tripTreatmentService = new TripTreatmentService();

            // Act
            var foundDetails = await tripDetailService.GetDetailsByTripId(tripId);

            var availablePlaces = tripTreatmentService.GetAvailablePlaces(foundDetails.Data,capacity);

            // Assert
            Assert.Equal(capacity-count, availablePlaces.Data.Count);
        }

        private List<TripDetails> GetTripDetails()
        {
            return new List<TripDetails>()
            {
                new TripDetails(){ Place = 1 },
                new TripDetails(){ Place = 3 },
                new TripDetails(){ Place = 5 },
                new TripDetails(){ Place = 6 },
            };
        }
    }
}
