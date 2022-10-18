using Booking.DAL.Interfaces;
using Booking.Domain.Enums;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;
using System.Threading.Tasks;

namespace Booking.Service.Implementations
{
    public class PlaneService : IPlaneService
    {
        private readonly IPlaneRepository _planeRepository;

        public PlaneService(IPlaneRepository planeRepository)
        {
            _planeRepository = planeRepository;
        }

        public async Task<BaseResponse<bool>> Create(Plane model)
        {
            try
            {

                var plane = await _planeRepository.GetByPlaneName(model.Name);

                if (plane != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A plane with this name already exists"
                    };
                }

                await _planeRepository.Create(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The plane was successfully added.",
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

                var plane = await _planeRepository.GetById(id);

                if (plane == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A plane with this id doesn't exist"
                    };
                }

                await _planeRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The plane was successfully removed",
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

        public async Task<BaseResponse<List<Plane>>> GetAll()
        {
            try
            {
                var planes = await _planeRepository.GetAll();

                return new BaseResponse<List<Plane>>()
                {
                    Data = planes,
                    Description = "Data received successfully",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Plane>>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Plane>> GetById(int id)
        {
            try
            {
                var plane = await _planeRepository.GetById(id);
                if (plane == null)
                {
                    return new BaseResponse<Plane>()
                    {
                        Data = null,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A plane with this id doesn't exist"
                    };
                }

                return new BaseResponse<Plane>()
                {
                    Data = plane,
                    Description = "The plane was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Plane>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Plane>> GetByName(string planeName)
        {
            try
            {
                var plane = await _planeRepository.GetByPlaneName(planeName);
                if (plane == null)
                {
                    return new BaseResponse<Plane>()
                    {
                        Data = null,
                        StatusCode = StatusCode.DataAlreadyExists,
                        Description = "A plane with this id doesn't exist"
                    };
                }

                return new BaseResponse<Plane>()
                {
                    Data = plane,
                    Description = "The plane was successfully found.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Plane>()
                {
                    Data = null,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Update(Plane model)
        {
            try
            {
                await _planeRepository.Update(model);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "The plane information was successfully updates.",
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
