using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces.Repositories;

namespace Booking.Service.Implementations.Repositories
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<BaseResponse<bool>> Create(Country model)
        {
            try
            {
                var plane = await _countryRepository.GetByName(model.Name);

                if (plane != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A country with this name already exists"
                    };
                }

                await _countryRepository.Create(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The country was successfully added.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteById(int id)
        {
            try
            {
                var plane = await _countryRepository.GetById(id);

                if (plane == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.CountryNotFound,
                        Description = "A country with this id doesn't exist"
                    };
                }

                await _countryRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The country was successfully removed",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<Country>>> GetAll()
        {
            try
            {
                var roles = await _countryRepository.GetAll();

                return new BaseResponse<List<Country>>()
                {
                    Data = roles,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Country>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<City>>> GetAllCities(int countryId)
        {
            try
            {
                var city = await _countryRepository.GetById(countryId);

                if (city == null)
                {
                    return new BaseResponse<List<City>>()
                    {
                        Data = null,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A country with this id doesn't exist"
                    };
                }

                var roles = await _countryRepository.GetAllCities(countryId);

                return new BaseResponse<List<City>>()
                {
                    Data = roles,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<City>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Country>> GetById(int id)
        {
            try
            {
                var country = await _countryRepository.GetById(id);
                if (country == null)
                {
                    return new BaseResponse<Country>()
                    {
                        Data = null,
                        StatusCode = StatusCode.CountryNotFound,
                        Description = "A country with this id doesn't exist"
                    };
                }

                return new BaseResponse<Country>()
                {
                    Data = country,
                    Description = "The country was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Country>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Country>> GetByName(string name)
        {
            try
            {
                var country = await _countryRepository.GetByName(name);
                if (country == null)
                {
                    return new BaseResponse<Country>()
                    {
                        Data = null,
                        StatusCode = StatusCode.CountryNotFound,
                        Description = "A country with this id doesn't exist"
                    };
                }

                return new BaseResponse<Country>()
                {
                    Data = country,
                    Description = "The country was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Country>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(Country model)
        {
            try
            {
                var data = _countryRepository.GetById(model.Id);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.CountryNotFound,
                        Description = "A country with this id doesn't exist"
                    };
                }

                await _countryRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The country information was successfully updates.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
