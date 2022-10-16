using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Repositories.MongoDB
{
    public class DeletedUserRepository : IBaseRepository<DeletedUser>
    {
        public Task<bool> Create(DeletedUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DeletedUser>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DeletedUser> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(DeletedUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
