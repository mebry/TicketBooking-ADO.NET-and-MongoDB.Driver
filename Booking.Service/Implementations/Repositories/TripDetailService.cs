using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces.Repositories;

namespace Booking.Service.Implementations.Repositories
{
    public class TripDetailService : ITripDetailService
    {
        private readonly ITripDetailRepository _tripDetailRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;

        public TripDetailService(ITripDetailRepository tripDetailRepository, ITripRepository tripRepository,
            IUserRepository userRepository)
        {
            _tripDetailRepository = tripDetailRepository;
            _tripRepository = tripRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Create(TripDetails model)
        {
            try
            {
                if (model == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataIsNotValid,
                        Description = "A trip detail cannot be null"
                    };
                }

                var trip = await _tripRepository.GetById(model.TripId);

                if (trip == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.TripNotFound,
                        Description = "A trip with this id doesn't exist"
                    };
                }

                var user = await _userRepository.GetById(model.UserId);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _tripDetailRepository.Create(model);

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
                var trip = await _tripDetailRepository.GetById(id);

                if (trip == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.TripDetailsNotFound,
                        Description = "A trip detail with this id doesn't exist"
                    };
                }

                await _tripDetailRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The trip detail was successfully removed",
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

        public async Task<BaseResponse<bool>> DeleteByPlace(int tripId, int place)
        {
            try
            {
                if (place <= 0)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataIsNotValid,
                        Description = "A trip with this place doesn't exist"
                    };
                }

                var trip = await _tripRepository.GetById(tripId);

                if (trip == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.TripDetailsNotFound,
                        Description = "A trip with this id doesn't exist"
                    };
                }

                var data = await _tripDetailRepository.DeleteByPlace(tripId, place);

                return new BaseResponse<bool>()
                {
                    Data = data,
                    StatusCode = StatusCode.TripNotFound,
                    Description = "A trip detail was deleted"
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

        public async Task<BaseResponse<List<TripDetails>>> GetAll()
        {
            try
            {
                var trips = await _tripDetailRepository.GetAll();

                return new BaseResponse<List<TripDetails>>()
                {
                    Data = trips,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TripDetails>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<TripDetails>> GetById(int id)
        {
            try
            {
                var trips = await _tripDetailRepository.GetById(id);

                return new BaseResponse<TripDetails>()
                {
                    Data = trips,
                    Description = "The trip was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TripDetails>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<TripDetails>>> GetDetailsByTripId(int tripId)
        {
            try
            {
                var trips = await _tripDetailRepository.GetDetailsByTripId(tripId);

                return new BaseResponse<List<TripDetails>>()
                {
                    Data = trips,
                    Description = "Trip details was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TripDetails>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(TripDetails model)
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

                await _tripDetailRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The trip detail was successfully updates.",
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

        private async Task<bool> IsExistTrip(int tripId)
        {
            var trip = await _tripRepository.GetById(tripId);

            if (trip == null)
            {
                return false;
            }

            return true;
        }
    }
}
