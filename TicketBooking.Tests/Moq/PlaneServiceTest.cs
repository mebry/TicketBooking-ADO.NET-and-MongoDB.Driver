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

namespace TicketBooking.Tests.Moq
{
    public class PlaneServiceTest
    {
        [Fact]
        public async Task CreateANewPlane()
        {
            // Arrange
            var newPlane = new Plane() { Name = "Ty-134", Capacity = 90 };

            var repositoryMock = new Mock<IPlaneRepository>();
            repositoryMock.Setup(repo => repo.Create(newPlane))
                .ReturnsAsync(true);

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.Create(newPlane);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task GetAllPlanes()
        {
            // Arrange
            var planes = GetPlanes();
            int count = planes.Count;

            var repositoryMock = new Mock<IPlaneRepository>();
            repositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(planes);

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.Equal(count, result.Data.Count);
        }

        [Fact]
        public async Task GetPlaneById()
        {
            // Arrange
            var planes = GetPlanes();
            int planeId = planes[0].Id;

            var repositoryMock = new Mock<IPlaneRepository>();
            repositoryMock.Setup(repo => repo.GetById(planeId))
                .ReturnsAsync(new Plane() { Id=1});

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.GetById(planeId);

            // Assert
            Assert.Equal(planeId, result.Data.Id);
        }

        [Fact]
        public async Task GetPlaneById_NotFound()
        {
            // Arrange
            var planes = GetPlanes();

            int planeId = planes[0].Id;
            int incorrectId = planes.Max(i => i.Id) + 1;

            var repositoryMock = new Mock<IPlaneRepository>();
            repositoryMock.Setup(repo => repo.GetById(planeId))
                .ReturnsAsync(new Plane() { Id = 1 });

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.GetById(incorrectId);

            // Assert
            Assert.NotEqual(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetPlaneByName()
        {
            // Arrange
            var planes = GetPlanes();
            string planeName = planes[0].Name;

            var repositoryMock = new Mock<IPlaneRepository>();
            repositoryMock.Setup(repo => repo.GetByPlaneName(planeName))
                .ReturnsAsync(new Plane() { Name = planeName });

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.GetByName(planeName);

            // Assert
            Assert.Equal(planeName, result.Data.Name);
        }

        [Fact]
        public async Task GetPlaneByName_NotFound()
        {
            // Arrange
            var planes = GetPlanes();
            string planeName = planes[0].Name;

            var repositoryMock = new Mock<IPlaneRepository>();
            repositoryMock.Setup(repo => repo.GetByPlaneName(planeName))
                .ReturnsAsync(new Plane() { Name = planeName });

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.GetByName("Something");

            // Assert
            Assert.NotEqual(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task DeletePlaneById()
        {
            // Arrange
            List<Plane> planes = GetPlanes();
            int planeId = planes[0].Id; //1
            var repositoryMock = new Mock<IPlaneRepository>();

            repositoryMock.Setup(repo => repo.GetById(planeId))
                    .ReturnsAsync(planes[0]);

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.DeleteById(planeId);

            // Assert
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task UpdateBlogPlaneName()
        {
            // Arrange
            List<Plane> planes = GetPlanes();
            int planeId = planes[0].Id; //1
            var plane = planes[0];

            plane.Name = "NewName";
            var repositoryMock = new Mock<IPlaneRepository>();

            repositoryMock.Setup(repo => repo.GetById(planeId))
                    .ReturnsAsync(planes[0]);

            IPlaneService service = new PlaneService(repositoryMock.Object);

            // Act
            var result = await service.Update(plane);

            // Assert
            Assert.Equal(true, result.Data);
        }

        private List<Plane> GetPlanes()
        {
            return new List<Plane>()
            {
                new Plane() {Id=1, Name = "Ty-134", Capacity = 90 },
                new Plane() {Id=2, Name = "Ty-154", Capacity = 100 },
                new Plane() {Id=3, Name = "Ty-164", Capacity = 85 }
            };
        }
    }
}