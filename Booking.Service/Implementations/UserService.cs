using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Helpers;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<List<User>>> GetAll()
        {
            try
            {
                var roles = await _userRepository.GetAll();

                return new BaseResponse<List<User>>()
                {
                    Data = roles,
                    Description = "Users received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<User>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<User>> GetById(int id)
        {
            try
            {
                var role = await _userRepository.GetById(id);

                if (role == null)
                {
                    return new BaseResponse<User>()
                    {
                        Data = null,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                return new BaseResponse<User>()
                {
                    Data = role,
                    Description = "The user was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(User model)
        {
            try
            {
                model.Password = HashPasswordHelper.HashPassowrd(model.Password);

                await _userRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The data was successfully updated.",
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
