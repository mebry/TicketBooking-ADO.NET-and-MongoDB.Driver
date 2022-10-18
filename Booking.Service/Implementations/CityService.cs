using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;

namespace Booking.Service.Implementations
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<BaseResponse<bool>> Create(City model)
        {
            try
            {
                var city = await _cityRepository.GetByName(model.Name);

                if (city != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A city with this name already exists"
                    };
                }

                await _cityRepository.Create(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The city was successfully added.",
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
                var city = await _cityRepository.GetById(id);

                if (city == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A city with this id doesn't exist"
                    };
                }

                await _cityRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The city was successfully removed",
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

        public async Task<BaseResponse<List<City>>> GetAll()
        {
            try
            {
                var cities = await _cityRepository.GetAll();

                return new BaseResponse<List<City>>()
                {
                    Data = cities,
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

        public async Task<BaseResponse<City>> GetByCityName(string name)
        {
            try
            {
                var city = await _cityRepository.GetByName(name);
                if (city == null)
                {
                    return new BaseResponse<City>()
                    {
                        Data = null,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A city with this id doesn't exist"
                    };
                }

                return new BaseResponse<City>()
                {
                    Data = city,
                    Description = "The city was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<City>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<City>> GetById(int id)
        {
            try
            {
                var plane = await _cityRepository.GetById(id);
                if (plane == null)
                {
                    return new BaseResponse<City>()
                    {
                        Data = null,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A city with this id doesn't exist"
                    };
                }

                return new BaseResponse<City>()
                {
                    Data = plane,
                    Description = "The city was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<City>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(City model)
        {
            try
            {
                await _cityRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The city information was successfully updates.",
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
