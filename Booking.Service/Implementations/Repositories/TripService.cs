using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces.Repositories;

namespace Booking.Service.Implementations.Repositories
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<BaseResponse<bool>> Create(Trip model)
        {
            try
            {
                if (model == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataIsNotValid,
                        Description = "A trip cannot be null"
                    };
                }

                await _tripRepository.Create(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The trip was successfully added.",
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
                var trip = await _tripRepository.GetById(id);

                if (trip == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.TripNotFound,
                        Description = "A trip with this id doesn't exist"
                    };
                }

                await _tripRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The trip was successfully removed",
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

        public async Task<BaseResponse<List<Trip>>> GetAll()
        {
            try
            {
                var trips = await _tripRepository.GetAll();

                return new BaseResponse<List<Trip>>()
                {
                    Data = trips,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Trip>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Trip>> GetById(int id)
        {
            try
            {
                var data = await _tripRepository.GetById(id);
                if (data == null)
                {
                    return new BaseResponse<Trip>()
                    {
                        Data = null,
                        StatusCode = StatusCode.TripNotFound,
                        Description = "A trip with this id doesn't exist"
                    };
                }

                return new BaseResponse<Trip>()
                {
                    Data = data,
                    Description = "The trip was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Trip>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(Trip model)
        {
            try
            {
                var data = _tripRepository.GetById(model.Id);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.TripNotFound,
                        Description = "A trip with this id doesn't exist"
                    };
                }

                await _tripRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The trip information was successfully updates.",
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
