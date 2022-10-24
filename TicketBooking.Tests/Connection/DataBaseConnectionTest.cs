using Booking.DAL.Interfaces;
using Booking.DAL.Repositories.MongoDB;
using Booking.DAL.Repositories.MsServer;
using Booking.Domain.Enums;
using Booking.Service.Implementations.Repositories;
using Booking.Service.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TicketBooking.Tests.Connection
{
    public class DataBaseConnectionTest
    {
        [Fact]
        public async Task TestMSServer_WithTheIncorrectPath_ThrowsException()
        {
            // Arrange
            string connectionString = "path";
            int planeId = -1;
            IPlaneRepository planeRepository = new PlaneRepositoryMS(connectionString);
            IPlaneService planeService = new PlaneService(planeRepository);
            // Act
            var result = await planeService.GetById(planeId);

            // Assert
            Assert.Equal(StatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task TestMSServer_WithTheCorrectPath_IsEqual()
        {
            // Arrange
            string connectionString = @"Data Source=DESKTOP-SEHI244;Initial Catalog=TicketBooking;Integrated Security=True";
            int planeId = -1;

            IPlaneRepository planeRepository = new PlaneRepositoryMS(connectionString);
            IPlaneService planeService = new PlaneService(planeRepository);

            // Act
            var result = await planeService.GetById(planeId);

            // Assert
            Assert.Equal(StatusCode.PlaneNotFound, result.StatusCode);
        }

        [Fact]
        public async Task TestMongoDB_WithTheIncorrectPath_ThrowsException()
        {
            string connectionString = "path";
            int planeId = -1;

            try
            {
                IPlaneRepository planeRepository = new PlaneRepository(connectionString);
                IPlaneService planeService = new PlaneService(planeRepository);

                var result = await planeService.GetById(planeId);
            }
            catch (Exception ex)
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
        }

        [Fact]
        public async Task TestMongoDB_WithTheCorrectPath_IsEqual()
        {
            // Arrange
            string connectionString = "mongodb://localhost:27017";
            int planeId = -1;

            IPlaneRepository planeRepository = new PlaneRepository(connectionString);
            IPlaneService planeService = new PlaneService(planeRepository);

            // Act
            var result = await planeService.GetById(planeId);

            // Assert
            Assert.Equal(StatusCode.PlaneNotFound, result.StatusCode);
        }
    }
}
