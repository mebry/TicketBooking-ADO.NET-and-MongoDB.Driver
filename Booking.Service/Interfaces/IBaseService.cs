using Booking.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interfaces
{
    public interface IBaseService<T>
    {
        Task<BaseResponse<bool>> Create(T model);
        Task<BaseResponse<bool>> Update(T model);
        Task<BaseResponse<bool>> DeleteById(int id);
        Task<BaseResponse<T>> GetById(int id);
        Task<BaseResponse<List<T>>> GetAll();
    }
}
