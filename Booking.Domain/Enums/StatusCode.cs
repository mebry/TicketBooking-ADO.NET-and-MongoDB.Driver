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
        CityNotFound,
        CountryNotFound,
        AgeIsNotValid,
        DataIsNotValid,
        TripDetailsNotFound,
        OK = 200,
        DataAlreadyExists=300,
        InternalServerError = 500,
    }
}
