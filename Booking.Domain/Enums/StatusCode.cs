using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Enums
{
    public enum StatusCode
    {
        UserNotFound = 1,
        UserAlreadyExists,
        IncorrectPassword,
        IncorrectCodeword,
        RoleNotFound,
        PlaneNotFound,
        TripNotFound,
        CitynotFound,
        CountryNotFound,
        OK = 200,
        InternalServerError = 500,
    }
}
