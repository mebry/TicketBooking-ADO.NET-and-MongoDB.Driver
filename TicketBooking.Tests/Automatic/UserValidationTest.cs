using Booking.Domain.Enums;
using Booking.Service.Implementations.Validations;
using Booking.Service.Interfaces.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TicketBooking.Tests.Automatic
{
    public class UserValidationTest
    {
        [Fact]
        public void IsValidAge_LessThenMin_IsFalse()
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();
            int yearOfBirth = -1;
            // Act
            var result= userValidation.IsValidAge(yearOfBirth);

            // Assert
            Assert.False(StatusCode.OK == result.StatusCode);
        }

        [Fact]
        public void IsValidAge_MoreThenMax_IsFalse()
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();

            int yearOfBirth = 3000;
            // Act
            var result = userValidation.IsValidAge(yearOfBirth);

            // Assert
            Assert.False(StatusCode.OK == result.StatusCode);
        }

        [Fact]
        public void IsValidAge_CheckMaxAge_IsTrue()
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();

            int yearOfBirth = DateTime.Now.Year - 120;
            // Act
            var result = userValidation.IsValidAge(yearOfBirth);

            // Assert
            Assert.True(StatusCode.OK == result.StatusCode);
        }

        [Fact]
        public void IsValidAge_CheckMinAge_IsTrue()
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();

            int yearOfBirth = DateTime.Now.Year - 12;
            // Act
            var result = userValidation.IsValidAge(yearOfBirth);

            // Assert
            Assert.True(StatusCode.OK == result.StatusCode);
        }

        [Fact]
        public void IsValidAge_CheckMidAge_IsTrue()
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();

            int yearOfBirth = DateTime.Now.Year - 64;
            // Act
            var result = userValidation.IsValidAge(yearOfBirth);

            // Assert
            Assert.True(StatusCode.OK == result.StatusCode);
        }

        [Theory]
        [InlineData("Mik1")]
        [InlineData("1")]
        [InlineData("Q")]
        [InlineData("")]
        [InlineData("_Mick_")]
        [InlineData("Nikc?")]
        [InlineData(null)]
        [InlineData("sd!")]
        [InlineData("123")]
        public void IsValidUserInfo_IncorrectData_IsFalse(string str)
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();

            // Act
            var result = userValidation.IsValidUserInfo(str);

            // Assert
            Assert.False(StatusCode.OK == result.StatusCode);
        }

        [Theory]
        [InlineData("Mike")]
        [InlineData("Zi")]
        [InlineData("Vadim")]
        public void IsValidUserInfo_CorrectData_IsTrue(string str)
        {
            // Arrange
            IUserValidation userValidation = new UserValidation();

            // Act
            var result = userValidation.IsValidUserInfo(str);

            // Assert
            Assert.True(StatusCode.OK == result.StatusCode);
        }
    }
}
