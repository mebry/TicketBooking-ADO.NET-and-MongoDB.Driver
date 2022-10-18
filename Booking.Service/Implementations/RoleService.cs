using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;

namespace Booking.Service.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<BaseResponse<bool>> Create(Role model)
        {
            try
            {

                var role = await _roleRepository.GetByName(model.Name);

                if (role != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A role with this name already exists"
                    };
                }

                await _roleRepository.Create(model);

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
                var role = await _roleRepository.GetById(id);

                if (role == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                await _roleRepository.Delete(id);

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

        public async Task<BaseResponse<List<Role>>> GetAll()
        {
            try
            {
                var roles = await _roleRepository.GetAll();

                return new BaseResponse<List<Role>>()
                {
                    Data = roles,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Role>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Role>> GetById(int id)
        {
            try
            {
                var role = await _roleRepository.GetById(id);
                if (role == null)
                {
                    return new BaseResponse<Role>()
                    {
                        Data = null,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                return new BaseResponse<Role>()
                {
                    Data = role,
                    Description = "The role was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Role>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Role>> GetByName(string name)
        {
            try
            {
                var role = await _roleRepository.GetByName(name);
                if (role == null)
                {
                    return new BaseResponse<Role>()
                    {
                        Data = null,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                return new BaseResponse<Role>()
                {
                    Data = role,
                    Description = "The role was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Role>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(Role model)
        {
            try
            {
                var data = _roleRepository.GetById(model.Id);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A role with this id doesn't exist"
                    };
                }

                await _roleRepository.Update(model);

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
