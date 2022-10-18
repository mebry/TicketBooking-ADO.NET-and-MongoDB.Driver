using Booking.Domain.Models;

namespace Booking.DAL.Interfaces
{
    public interface IRoleRepository:IBaseRepository<Role>
    {
        Task<Role> GetByName(string name);
    }
}
