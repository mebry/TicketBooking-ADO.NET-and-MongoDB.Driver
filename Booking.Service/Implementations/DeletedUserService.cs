using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;

namespace Booking.Service.Implementations
{
    public class DeletedUserService: IDeletedUserService
    {
        private readonly IDeletedUserRepository _deletedUserRepository;

        public DeletedUserService(IDeletedUserRepository deletedUserRepository)
        {
            _deletedUserRepository = deletedUserRepository;
        }

        public async Task<BaseResponse<bool>> Create(DeletedUser model)
        {
            try
            {
                var user = await _deletedUserRepository.GetById(model.Id);

                if (user != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A user with this name already exists"
                    };
                }

                await _deletedUserRepository.Create(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The user was successfully added.",
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
                var user = await _deletedUserRepository.GetById(id);

                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.RoleNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _deletedUserRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The user was successfully removed",
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

        public async Task<BaseResponse<List<DeletedUser>>> GetAll()
        {
            try
            {
                var users = await _deletedUserRepository.GetAll();

                return new BaseResponse<List<DeletedUser>>()
                {
                    Data = users,
                    Description = "Data received successfully.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<DeletedUser>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<DeletedUser>> GetById(int id)
        {
            try
            {
                var role = await _deletedUserRepository.GetById(id);

                if (role == null)
                {
                    return new BaseResponse<DeletedUser>()
                    {
                        Data = null,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                return new BaseResponse<DeletedUser>()
                {
                    Data = role,
                    Description = "The user was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<DeletedUser>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(DeletedUser model)
        {
            try
            {
                var data = _deletedUserRepository.GetById(model.Id);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _deletedUserRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The user information was successfully updates.",
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

        public async Task<BaseResponse<bool>> UpdateTheReason(int userId, string reason)
        {
            try
            {
                var data = _deletedUserRepository.GetById(userId);

                if (data == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UserNotFound,
                        Description = "A user with this id doesn't exist"
                    };
                }

                await _deletedUserRepository.UpdateTheReason(userId, reason);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The reason S was successfully updates.",
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
