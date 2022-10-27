using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Service.Implementations.Calculations;
using Booking.Service.Interfaces.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TicketBooking.Tests.Automatic
{
    public class TripTreatmentServiceTest
    {
        [Fact]
        public void GetAvailablePlaces_NullData_IsFalse()
        {
            // Arrange
            ITripTreatmentService tripTreatmentService = new TripTreatmentService();
            List<TripDetails> tripDetails = null;
            int capacity = 10;
            // Act
            var result = tripTreatmentService.GetAvailablePlaces(tripDetails, capacity);

            // Assert
            Assert.False(StatusCode.OK == result.StatusCode);
        }

        [Fact]
        public void GetAvailablePlaces_IncorrentCount_IsFalse()
        {
            // Arrange
            ITripTreatmentService tripTreatmentService = new TripTreatmentService();
            List<TripDetails> tripDetails = new List<TripDetails>()
            {
                new TripDetails(){Place=1},
                new TripDetails(){Place=3},
                new TripDetails(){Place=2},
                new TripDetails(){Place=5},
                new TripDetails(){Place=10},
            };
            int capacity = 10;
            int available = 4;
            // Act
            var result = tripTreatmentService.GetAvailablePlaces(tripDetails, capacity);

            // Assert
            Assert.False(available == result.Data.Count);
        }

        [Fact]
        public void GetAvailablePlaces_IncorrcentAvailablePlaces_IsFalse()
        {
            // Arrange
            ITripTreatmentService tripTreatmentService = new TripTreatmentService();
            List<TripDetails> tripDetails = new List<TripDetails>()
            {
                new TripDetails(){Place=1},
                new TripDetails(){Place=3},
                new TripDetails(){Place=2},
                new TripDetails(){Place=5},
                new TripDetails(){Place=10},
            };
            int capacity = 10;
            int available = 5;
            // Act
            var result = tripTreatmentService.GetAvailablePlaces(tripDetails, capacity);

            // Assert
            Assert.False(available+1 == result.Data.Count);
        }

        [Fact]
        public void GetAvailablePlaces_CorrentCount_IsTrue()
        {
            // Arrange
            ITripTreatmentService tripTreatmentService = new TripTreatmentService();
            List<TripDetails> tripDetails = new List<TripDetails>()
            {
                new TripDetails(){Place=1},
                new TripDetails(){Place=3},
                new TripDetails(){Place=2},
                new TripDetails(){Place=5},
                new TripDetails(){Place=10},
            };
            int capacity = 10;
            int available = 5;
            // Act
            var result = tripTreatmentService.GetAvailablePlaces(tripDetails, capacity);

            // Assert
            Assert.True(available == result.Data.Count);
        }
    }
}
