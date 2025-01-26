using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessment.Models;

namespace TestAssessment.Services.IServices
{
    public interface ITripService
    {
        List<int> ProcessTripsFromCsv(string filePath, string duplicatesFilePath);
        PULocationIDWithHighestTip GetPULocationWithHighestTip();

        List<Trip> GetTop100LongestTripsByDistance();

        List<TripWithDuration> GetTop100LongestTripsByDuration();

        List<Trip> GetTripsByPULocation(int puLocationId);
    }
}
