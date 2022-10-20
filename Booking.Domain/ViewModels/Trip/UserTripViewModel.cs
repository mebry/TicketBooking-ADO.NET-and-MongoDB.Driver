using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ViewModels.Trip
{
    public class UserTripViewModel
    {
        public int TripId { get; set; }

        public string StartCity { get; set; }

        public string EndCity { get; set; }

        public string StartCountry { get; set; }

        public string EndCountry { get; set; }

        public int Place { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
