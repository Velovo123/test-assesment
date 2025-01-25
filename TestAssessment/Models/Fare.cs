using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssessment.Models
{
    public class Fare
    {
        public int FareId { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }
        public int TripId { get; set; }
    }
}
