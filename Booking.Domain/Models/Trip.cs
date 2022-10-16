using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public class Trip
    {
        public int TripId { get; set; }

        public string Plane { get; set; }

        public string StartCity { get; set; }

        public string StartCountry { get; set; }

        public int Capacity { get; set; }

        public int Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string EndCity { get; set; }

        public string EndCountry { get; set; }
    }
}
