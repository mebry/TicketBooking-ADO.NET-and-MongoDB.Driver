using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Helpers;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;
using Booking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AccountService(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = await _userRepository.GetByUserName(model.UserName);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "User not found",
                    };
                }

                if (model.Codeword != user.Codeword)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.IncorrectCodeword,
                        Description = "Incorrect codeword",
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassowrd(model.OldPassword))
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.IncorrectPassword,
                        Description = "Passwords don't match",
                    };
                }

                await _userRepository.UpdatePassword(user.Id, HashPasswordHelper.HashPassowrd(model.NewPassword));

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The password was successfully changed",
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

        public async Task<BaseResponse<bool>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetByUserName(model.UserName);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "User not found",
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassowrd(model.Password))
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.IncorrectPassword,
                        Description = "Passwords don't match",
                    };
                }

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Successful login",
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

        public async Task<BaseResponse<bool>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _userRepository.GetByUserName(model.UserName);

                if (user != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserAlreadyExists,
                        Description = "A user with this username already exists",
                    };
                }

                if (model.Password != model.PasswordConfirm)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.IncorrectPassword,
                        Description = "Passwords don't match",
                    };
                }

                var newUser = new User()
                {
                    UserName = model.UserName,
                    Password = HashPasswordHelper.HashPassowrd(model.Password),
                    Codeword = model.Codeword,
                };

                await _userRepository.Create(newUser);

                var foundUser = await _userRepository.GetByUserName(newUser.UserName);

                await _userRoleRepository.Create(new UserRole()
                {
                    UserId = foundUser.Id,
                    RoleId = 1
                });

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The user was added",
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
