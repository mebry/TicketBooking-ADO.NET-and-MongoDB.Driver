using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Repositories
{
    public interface IDeletedUserService : IBaseService<DeletedUser>
    {
        Task<BaseResponse<bool>> UpdateTheReason(int userId, string reason);
    }
}
