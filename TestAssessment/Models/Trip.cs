using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssessment.Models
{
    public class Trip
    {
        public int TripId { get; set; }
        public DateTime PickupDatetime { get; set; }
        public DateTime DropoffDatetime { get; set; }
        public int PassengerCount { get; set; }
        public double TripDistance { get; set; }
        public string StoreAndFwdFlag { get; set; }

        public Trip(DateTime pickupDatetime, DateTime dropoffDatetime, int passengerCount, double tripDistance, string storeAndFwdFlag)
        {
            PickupDatetime = pickupDatetime;
            DropoffDatetime = dropoffDatetime;
            PassengerCount = passengerCount;
            TripDistance = tripDistance;
            StoreAndFwdFlag = storeAndFwdFlag;
        }

        public Trip()
        {
            StoreAndFwdFlag = string.Empty;
        }
    }
}
