using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Repositories
{
    public interface IRoleService: IBaseService<Role>
    {
        Task<BaseResponse<Role>> GetByName(string name);
    }
}
