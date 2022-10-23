using Booking.Domain.Enums;
using Booking.Domain.Responses;
using Booking.Service.Interfaces.Calculations;
using System.Text.RegularExpressions;

namespace Booking.Service.Implementations.Validations
{
    public class UserValidation : IUserValidation
    {
        public BaseResponse<bool> IsValidAge(int yearOfBirth)
        {
            if (yearOfBirth >= 12)
            {
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "The age is valid."
                };
            }

            return new BaseResponse<bool>()
            {
                Data = false,
                StatusCode = StatusCode.AgeIsNotValid,
                Description = "The age isn't valid."
            };
        }

        public BaseResponse<bool> IsValidUserInfo(string data)
        {
            if(!string.IsNullOrEmpty(data) && !Regex.IsMatch(data, @"[^\w\.@!?()%$+=#%&*~1234567890<>]"))
            {
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "The data is valid."
                };
            }

            return new BaseResponse<bool>()
            {
                Data = false,
                StatusCode = StatusCode.DataIsNotValid,
                Description = "The data isn't valid."
            };
        }
    }
}
