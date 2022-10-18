using Booking.Domain.Models;

namespace Booking.DAL.Interfaces
{
    public interface IUserRoleRepository:IBaseRepository<UserRole>
    {
        Task<List<UserRole>> GetByUserName(string username);
    }
}
