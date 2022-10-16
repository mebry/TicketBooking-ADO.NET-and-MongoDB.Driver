using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public class UserDelails
    {
        public int UserDelailsId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
