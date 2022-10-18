using Booking.Domain.Models;

namespace Booking.DAL.Interfaces
{
    public interface IUserDetailsRepository : IBaseRepository<UserDelails>
    {
        Task<bool> UpdateFirstName(int userId, string firstName);
        Task<bool> UpdateLastName(int userId, string lastName);
        Task<bool> UpdatePatronymic(int userId, string patronymic);
        Task<UserDelails> GetByUserName(string userName);
    }
}
