using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ViewModels.Trip
{
    public class TripInfo
    {
        public int TripId { get; set; }

        public string StartCity { get; set; }

        public string EndCity { get; set; }

        public string StartCountry { get; set; }

        public string EndCountry { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfOrderedTickets { get; set; }
        public int AllTictets { get; set; }

        public int Profit { get; set; }
    }
}
