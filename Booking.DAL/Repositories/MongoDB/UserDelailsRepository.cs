using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Repositories.MongoDB
{
    public class UserDelailsRepository : IBaseRepository<UserDelails>
    {
        
        public Task<bool> Create(UserDelails entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDelails>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserDelails> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(UserDelails entity)
        {
            throw new NotImplementedException();
        }
    }
}
