using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public class DeletedUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime DateTime { get; set; }

        public string Reason { get; set; }
    }
}
