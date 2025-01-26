using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessment.Models;

namespace TestAssessment.Repository.IRepository
{
    public interface ITripRepository
    {
        int InsertUnifiedTrip(Trip trip);

        IEnumerable<Trip> GetUnifiedTrips();

        Trip? GetUnifiedTripById(int tripId);

        void DeleteUnifiedTripById(int tripId);

        void BulkInsertUnifiedTrips(IEnumerable<Trip> trips, out List<int> generatedIds);

        PULocationIDWithHighestTip GetPULocationWithHighestTip();

        List<Trip> GetTop100LongestTripsByDistance();

        List<TripWithDuration> GetTop100LongestTripsByDuration();

        List<Trip> GetTripsByPULocation(int puLocationId);
    }
}
