using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using TestAssessment.Models;
using TestAssessment.Services.IServices;

namespace TestAssessment.Services
{
    public class DuplicateProcessor : IDuplicateProcessor
    {
        public (List<Trip> UniqueTrips, List<Trip> Duplicates) FindAndRemoveDuplicates(List<Trip> trips, string duplicatesFilePath)
        {
            var grouped = trips
                .GroupBy(t => new { t.PickupDatetime, t.DropoffDatetime, t.PassengerCount })
                .ToList();

            var uniqueTrips = new List<Trip>();
            var duplicates = new List<Trip>();

            foreach (var group in grouped)
            {
                uniqueTrips.Add(group.First()); 
                if (group.Count() > 1)
                {
                    duplicates.AddRange(group.Skip(1)); 
                }
            }

            WriteDuplicatesToCsv(duplicates, duplicatesFilePath);

            return (uniqueTrips, duplicates);
        }

        private void WriteDuplicatesToCsv(List<Trip> duplicates, string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            csv.WriteHeader<Trip>();
            csv.NextRecord();
            foreach (var duplicate in duplicates)
            {
                csv.WriteRecord(duplicate);
                csv.NextRecord();
            }
        }
    }
}
