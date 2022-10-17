using Booking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Interfaces
{
    public interface IUserRepository:IBaseRepository<User>
    {
        Task<User> GetByUserName(string userName);
        Task<bool> UpdatePassword(int userId,string newPassword);
    }
}
