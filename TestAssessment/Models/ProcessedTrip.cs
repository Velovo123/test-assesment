using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssessment.Models
{
    public class ProcessedTrip
    {
        public int ProcessedTripId { get; set; }
        public int TripId { get; set; }
        public bool IsDuplicate { get; set; }
        public DateTime? UTCConversionDatetime { get; set; }
    }
}
