using Booking.Domain.Models;

namespace Booking.DAL.Interfaces
{
    public interface IDeletedUserRepository: IBaseRepository<DeletedUser>
    {
        Task<bool> UpdateTheReason(int id, string reason);
    }
}
