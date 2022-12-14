using Booking.Domain.Models;
using Booking.Domain.Responses;

namespace Booking.Service.Interfaces.Repositories
{
    public interface IPlaneService:IBaseService<Plane>
    {
        Task<BaseResponse<Plane>> GetByName(string name);

    }
}
