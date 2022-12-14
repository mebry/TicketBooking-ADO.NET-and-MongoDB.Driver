using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Repositories
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        Task<BaseResponse<List<UserRole>>> GetByUserName(string name);
    }
}