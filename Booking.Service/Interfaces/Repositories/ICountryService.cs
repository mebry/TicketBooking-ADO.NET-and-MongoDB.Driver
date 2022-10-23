using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Repositories
{
    public interface ICountryService:IBaseService<Country>
    {
        Task<BaseResponse<Country>> GetByName(string name);
        Task<BaseResponse<List<City>>> GetAllCities(int countryId);
    }
}
