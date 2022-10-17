using Booking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Interfaces
{
    public interface IPlaneRepository : IBaseRepository<Plane>
    {
        Task<Plane> GetByPlaneName(string planeName);
    }
}
