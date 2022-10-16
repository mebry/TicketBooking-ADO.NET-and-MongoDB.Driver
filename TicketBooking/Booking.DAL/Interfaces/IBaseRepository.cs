using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> GetById(int id);

        Task<bool> Create(T entity);

        Task<List<T>> GetAll();

        Task<bool> Delete(T entity);

        Task<bool> Update(T entity);
    }
}
