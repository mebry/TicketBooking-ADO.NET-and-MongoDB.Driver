using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public  class TripDetails
    {
        public int TripDetailsId { get; set; }

        public int TripId { get; set; }

        public int UserId { get; set; }

        public int Place { get; set; }
    }
}
