using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Trip;
using Booking.Service.Interfaces.Repositories;

namespace Booking.Service.Implementations.Repositories
{
    public class TripInformationService : ITripInformationService
    {
        private readonly ITripInformationRepository _infoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITripRepository _tripRepository;

        public TripInformationService(ITripInformationRepository repository, IUserRepository userRepository,
            ITripRepository tripRepository)
        {
            _infoRepository = repository;
            _userRepository = userRepository;
            _tripRepository = tripRepository;
        }

        public async Task<BaseResponse<List<UserTripViewModel>>> GetExecutedTripsByUser(int userId)
        {
            try
            {

                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    return new BaseResponse<List<UserTripViewModel>>()
                    {
                        Data = null,
                        Description = "The user with this id doesn't exist",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = await _infoRepository.GetExecutedTripsByUser(userId);

                return new BaseResponse<List<UserTripViewModel>>()
                {
                    Data = data,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserTripViewModel>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<TripInfo>>> GetInformationByDate(int countDays)
        {
            try
            {
                if (countDays < 0)
                {
                    return new BaseResponse<List<TripInfo>>()
                    {
                        Data = null,
                        Description = "Incorrect count of days",
                        StatusCode = StatusCode.DataIsNotValid
                    };
                }

                var data = await _infoRepository.GetInformationByDate(countDays);

                return new BaseResponse<List<TripInfo>>()
                {
                    Data = data,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TripInfo>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<TripInfo>> GetInformationByTrip(int tripId)
        {
            try
            {
                var trip = _tripRepository.GetById(tripId);

                if (trip == null)
                {
                    return new BaseResponse<TripInfo>()
                    {
                        Data = null,
                        Description = "The trip with this id doesn't exist",
                        StatusCode = StatusCode.DataIsNotValid
                    };
                }

                var data = await _infoRepository.GetInformationByTrip(tripId);

                return new BaseResponse<TripInfo>()
                {
                    Data = data,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<TripInfo>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<TripInfo>>> GetInformationByTrips()
        {
            try
            {
                var data = await _infoRepository.GetInformationByTrips();

                return getValidResponse(data);
            }
            catch (Exception ex)
            {
                return getErrorResponse(null);
            }
        }

        public async Task<BaseResponse<List<TripInfo>>> GetInformationByTripsInside()
        {
            try
            {
                var data = await _infoRepository.GetInformationByTripsInside();

                return getValidResponse(data);
            }
            catch (Exception ex)
            {
                return getErrorResponse(null);
            }
        }

        public async Task<BaseResponse<List<TripInfo>>> GetInformationByTripsOutside()
        {
            try
            {
                var data = await _infoRepository.GetInformationByTripsOutside();

                return getValidResponse(data);
            }
            catch (Exception ex)
            {
                return getErrorResponse(null);
            }
        }

        public async Task<BaseResponse<List<UserTripViewModel>>> GetPlannedTripsByUser(int userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    return new BaseResponse<List<UserTripViewModel>>()
                    {
                        Data = null,
                        Description = "The user with this id doesn't exist",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = await _infoRepository.GetPlannedTripsByUser(userId);

                return new BaseResponse<List<UserTripViewModel>>()
                {
                    Data = data,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserTripViewModel>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<UserTripViewModel>>> GetTripsByUserId(int userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);

                if (user == null)
                {
                    return new BaseResponse<List<UserTripViewModel>>()
                    {
                        Data = null,
                        Description = "The user with this id doesn't exist",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = await _infoRepository.GetTripsByUserId(userId);

                return new BaseResponse<List<UserTripViewModel>>()
                {
                    Data = data,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserTripViewModel>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private BaseResponse<List<TripInfo>> getValidResponse(List<TripInfo> data)
            => new BaseResponse<List<TripInfo>>()
            {
                Data = data,
                Description = "Data received successfully.",
                StatusCode = StatusCode.OK
            };

        private BaseResponse<List<TripInfo>> getErrorResponse(List<TripInfo> data)
            => new BaseResponse<List<TripInfo>>()
            {
                Data = data,
                Description = "Data received successfully.",
                StatusCode = StatusCode.InternalServerError
            };

        private async Task<BaseResponse<List<TripInfo>>> GetRes(Handler handler)
        {
            try
            {
                var data = await handler?.Invoke();

                return new BaseResponse<List<TripInfo>>()
                {
                    Data = data,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TripInfo>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        private delegate Task<List<TripInfo>> Handler();
    }
}
