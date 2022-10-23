using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;

namespace Booking.Service.Interfaces.Repositories
{
    public interface IUserDetailsService 
    {
        Task<BaseResponse<bool>> Create(UserDelails model);
        Task<BaseResponse<bool>> Update(UserDelails model);
        Task<BaseResponse<bool>> DeleteByUserId(int id);
        Task<BaseResponse<UserDelails>> GetByUserId(int id);
        Task<BaseResponse<List<UserDelails>>> GetAll();
        Task<BaseResponse<UserDelails>> GetByUserName(string userName);
        Task<BaseResponse<bool>> UpdateFirstName(int userId, string firstName);
        Task<BaseResponse<bool>> UpdateLastName(int userId, string lastName);
        Task<BaseResponse<bool>> UpdatePatronymic(int userId, string patronymic);
    }
}
