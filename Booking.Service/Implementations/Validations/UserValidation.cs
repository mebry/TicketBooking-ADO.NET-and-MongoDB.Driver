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
            int age = DateTime.Now.Year - yearOfBirth;
            if (age >= 12 && age <= 120)
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
            string pattern = @".,@!?()_%$+=#%&*~1234567890<>";
            if (!string.IsNullOrEmpty(data) && data.Length > 1 && !data.Any(c => pattern.Contains(c)))
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
