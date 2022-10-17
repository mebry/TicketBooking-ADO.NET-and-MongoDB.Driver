using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public string PlaneId { get; set; }

        public int StartCityId { get; set; }

        public int EndCityId { get; set; }

        public int Capacity { get; set; }

        public int Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
