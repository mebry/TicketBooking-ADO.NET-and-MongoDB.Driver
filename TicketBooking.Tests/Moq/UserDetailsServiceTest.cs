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
using Booking.Service.Implementations.Validations;

namespace TicketBooking.Tests.Integrated
{
    public class UserDetailsServiceTest
    {
        [Fact]
        public async Task ValidationAgeAndCreateUserDetails_IsTrue()
        {
            // Arrange
            var userDetail = GetUserDelails();
            var userId = userDetail.UserId;

            var mock = new Mock<IUserDetailsRepository>();

            mock.Setup(repo => repo.Create(userDetail)).ReturnsAsync(true);
            mock.Setup(i => i.GetById(userId)).ReturnsAsync(new UserDelails());

            IUserDetailsService userDetailsService = new UserDetailsService(mock.Object);

            IUserValidation userValidation = new UserValidation();

            // Act
            BaseResponse<bool> result = new BaseResponse<bool>();
            if (userValidation.IsValidAge(userDetail.YearOfBirth).Data)
            {
                result = await userDetailsService.Create(userDetail);
            }
            else
            {
                Assert.True(false);
            }

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task CreateThanValidationStringInfoAndUpdate_IsTrue()
        {
            // Arrange
            var userDetail = GetUserDelails();
            var userId = userDetail.UserId;
            var newLastName = "Something";
            var mock = new Mock<IUserDetailsRepository>();

            mock.Setup(repo => repo.Create(userDetail)).ReturnsAsync(true);
            mock.Setup(i => i.UpdateLastName(userId, newLastName)).ReturnsAsync(true);
            mock.Setup(i => i.GetById(userId)).ReturnsAsync(new UserDelails());

            IUserDetailsService userDetailsService = new UserDetailsService(mock.Object);

            IUserValidation userValidation = new UserValidation();

            // Act
            await userDetailsService.Create(userDetail);
            BaseResponse<bool> result = new BaseResponse<bool>();
            if (userValidation.IsValidUserInfo(newLastName).Data)
            {
                result = await userDetailsService.UpdateLastName(userId, newLastName);
            }
            else
            {
                Assert.True(false);
            }

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }
        private UserDelails GetUserDelails()
            => new UserDelails()
            {
                FirstName = "f1",
                LastName = "f2",
                Patronymic = "f3",
                UserId = 1,
                YearOfBirth = 2000
            };


    }
}
