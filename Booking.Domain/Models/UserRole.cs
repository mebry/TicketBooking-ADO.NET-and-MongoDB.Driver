using Booking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public class UserRole
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }
    }
}
