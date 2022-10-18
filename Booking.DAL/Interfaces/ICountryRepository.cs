using Booking.Domain.Models;


namespace Booking.DAL.Interfaces
{
    public interface ICountryRepository:IBaseRepository<Country>
    {
        Task<Country> GetByName(string name);
        Task<List<City>> GetAllCities(int countryId);
    }
}
