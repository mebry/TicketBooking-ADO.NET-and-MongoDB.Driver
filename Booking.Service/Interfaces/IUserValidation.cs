﻿using Booking.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interfaces
{
    public interface IUserValidation
    {
        BaseResponse<bool> IsValidAge(int yearOfBirth);
        BaseResponse<bool> IsPasswordsMatch(string password,string comfirmPassword);
        BaseResponse<bool> IsValidUserInfo(string data);
    }
}
