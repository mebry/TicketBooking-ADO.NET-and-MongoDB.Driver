﻿using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces
{
    public interface ICityService : IBaseService<City>
    {
        Task<BaseResponse<City>> GetByCityName(string name);
    }
}
