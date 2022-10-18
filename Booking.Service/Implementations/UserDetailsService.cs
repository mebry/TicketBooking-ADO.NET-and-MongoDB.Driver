using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;

namespace Booking.Service.Implementations
{
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserDetailsService(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<BaseResponse<bool>> Create(UserDelails model)
        {
            try
            {
                var userDelails = await _userDetailsRepository.GetById(model.UserId);

                if (userDelails.FirstName != null || userDelails.LastName != null || userDelails.Patronymic != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A user details with this name already exists. Have to use an update method"
                    };
                }

                await _userDetailsRepository.Create(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The role was successfully added.",
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

        public async Task<BaseResponse<bool>> DeleteByUserId(int userId)
        {
            try
            {
                var userDelails = await _userDetailsRepository.GetById(userId);

                if (userDelails == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user details with this id doesn't exist"
                    };
                }

                await _userDetailsRepository.Delete(userId);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The information was successfully removed",
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

        public async Task<BaseResponse<List<UserDelails>>> GetAll()
        {
            try
            {
                var userDelails = await _userDetailsRepository.GetAll();

                return new BaseResponse<List<UserDelails>>()
                {
                    Data = userDelails,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserDelails>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<UserDelails>> GetByUserId(int id)
        {
            try
            {
                var userDelails = await _userDetailsRepository.GetById(id);
                if (userDelails == null)
                {
                    return new BaseResponse<UserDelails>()
                    {
                        Data = null,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "The User details with this id doesn't exist"
                    };
                }

                return new BaseResponse<UserDelails>()
                {
                    Data = userDelails,
                    Description = "The User details was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDelails>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<UserDelails>> GetByUserName(string userName)
        {
            try
            {
                var userDelails = await _userDetailsRepository.GetByUserName(userName);
                if (userDelails == null)
                {
                    return new BaseResponse<UserDelails>()
                    {
                        Data = null,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "The User details with this id doesn't exist"
                    };
                }

                return new BaseResponse<UserDelails>()
                {
                    Data = userDelails,
                    Description = "The User details was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDelails>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(UserDelails model)
        {
            try
            {
                var data = _userDetailsRepository.GetById(model.UserId);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                await _userDetailsRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The role information was successfully updates.",
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

        public async Task<BaseResponse<bool>> UpdateFirstName(int userId, string firstName)
        {
            try
            {
                var user = await _userDetailsRepository.GetById(userId);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _userDetailsRepository.UpdateFirstName(userId, firstName);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The information was successfully updates.",
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

        public async Task<BaseResponse<bool>> UpdateLastName(int userId, string lastName)
        {
            try
            {
                var user = await _userDetailsRepository.GetById(userId);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _userDetailsRepository.UpdateLastName(userId, lastName);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The information was successfully updates.",
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

        public async Task<BaseResponse<bool>> UpdatePatronymic(int userId, string patronymic)
        {
            try
            {
                var user = await _userDetailsRepository.GetById(userId);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _userDetailsRepository.UpdatePatronymic(userId, patronymic);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The information was successfully updates.",
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
