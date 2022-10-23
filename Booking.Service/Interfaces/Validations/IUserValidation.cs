using Booking.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interfaces.Calculations
{
    public interface IUserValidation
    {
        BaseResponse<bool> IsValidAge(int yearOfBirth);
        BaseResponse<bool> IsValidUserInfo(string data);
    }
}
