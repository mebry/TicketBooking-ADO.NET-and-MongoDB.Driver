using Booking.Domain.Models;
using Booking.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<User>> GetById(int id);
        Task<BaseResponse<List<User>>> GetAll();
        Task<BaseResponse<bool>> Update(User model);
    }
}
