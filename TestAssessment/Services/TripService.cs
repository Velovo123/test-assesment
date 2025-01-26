using TestAssessment.Repository.IRepository;
using TestAssessment.Services.IServices;
using TestAssessment.Models;

namespace TestAssessment.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _unifiedTripRepository;
        private readonly ICsvProcessor _csvProcessor;
        private readonly IDuplicateProcessor _duplicateProcessor;

        public TripService(
            ITripRepository unifiedTripRepository,
            ICsvProcessor csvProcessor,
            IDuplicateProcessor duplicateProcessor)
        {
            _unifiedTripRepository = unifiedTripRepository;
            _csvProcessor = csvProcessor;
            _duplicateProcessor = duplicateProcessor;
        }

        public List<int> ProcessTripsFromCsv(string filePath, string duplicatesFilePath)
        {
            var parsedData = _csvProcessor.ReadDataFromCsv(filePath);

            var (uniqueTrips, duplicates) = _duplicateProcessor.FindAndRemoveDuplicates(parsedData, duplicatesFilePath);

            _unifiedTripRepository.BulkInsertUnifiedTrips(uniqueTrips, out var generatedTripIds);

            return generatedTripIds;
        }

        public PULocationIDWithHighestTip GetPULocationWithHighestTip()
        {
            return _unifiedTripRepository.GetPULocationWithHighestTip();
        }

        public List<Trip> GetTop100LongestTripsByDistance()
        {
            return _unifiedTripRepository.GetTop100LongestTripsByDistance();
        }

        public List<TripWithDuration> GetTop100LongestTripsByDuration()
        {
            return _unifiedTripRepository.GetTop100LongestTripsByDuration();
        }

        public List<Trip> GetTripsByPULocation(int puLocationId)
        {
            return _unifiedTripRepository.GetTripsByPULocation(puLocationId);
        }
    }
}
