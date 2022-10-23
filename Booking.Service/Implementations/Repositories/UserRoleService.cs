using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces.Repositories;

namespace Booking.Service.Implementations.Repositories
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Create(UserRole model)
        {
            try
            {
                var user = await _userRepository.GetById(model.UserId);

                if (user != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this username doesn't exist"
                    };
                }

                _userRoleRepository.Create(model);

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

        public async Task<BaseResponse<bool>> DeleteById(int id)
        {
            try
            {
                var role = await _userRoleRepository.GetById(id);

                if (role == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                await _userRoleRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The role was successfully removed",
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

        public async Task<BaseResponse<List<UserRole>>> GetAll()
        {
            try
            {
                var roles = await _userRoleRepository.GetAll();

                return new BaseResponse<List<UserRole>>()
                {
                    Data = roles,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserRole>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<UserRole>> GetById(int id)
        {
            try
            {
                var role = await _userRoleRepository.GetById(id);
                if (role == null)
                {
                    return new BaseResponse<UserRole>()
                    {
                        Data = null,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                return new BaseResponse<UserRole>()
                {
                    Data = role,
                    Description = "The role was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserRole>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<List<UserRole>>> GetByUserName(string name)
        {
            try
            {
                var role = await _userRoleRepository.GetByUserName(name);

                if (role == null)
                {
                    return new BaseResponse<List<UserRole>>
                    {
                        Data = null,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                return new BaseResponse<List<UserRole>>()
                {
                    Data = role,
                    Description = "The role was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserRole>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(UserRole model)
        {
            try
            {
                var data = _userRoleRepository.GetById(model.Id);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                await _userRoleRepository.Update(model);

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
    }
}
