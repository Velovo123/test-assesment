using System;
using TestAssessment.Services.IServices;

namespace TestAssessment.UI
{
    public class TripMenu
    {
        private readonly ITripService _tripService;

        public TripMenu(ITripService tripService)
        {
            _tripService = tripService;
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Trip Processing Menu ===");
                Console.WriteLine("1. Process trips from CSV");
                Console.WriteLine("2. Display PULocationID with the highest average tip");
                Console.WriteLine("3. Display top 100 longest trips by distance");
                Console.WriteLine("4. Display top 100 longest trips by duration");
                Console.WriteLine("5. Search trips by PULocationID");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProcessTripsFromCsv();
                        break;
                    case "2":
                        DisplayHighestTipLocation();
                        break;
                    case "3":
                        DisplayTopTripsByDistance();
                        break;
                    case "4":
                        DisplayTopTripsByDuration();
                        break;
                    case "5":
                        SearchTripsByPULocation();
                        break;
                    case "6":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ProcessTripsFromCsv()
        {
            Console.Write("Enter the file path for the CSV: ");
            var filePath = Console.ReadLine();
            Console.Write("Enter the file path to save duplicates: ");
            var duplicatesFilePath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(duplicatesFilePath))
            {
                Console.WriteLine("Invalid file paths provided. Please try again.");
                return;
            }

            var insertedTripIds = _tripService.ProcessTripsFromCsv(filePath, duplicatesFilePath);
            if (insertedTripIds == null || insertedTripIds.Count == 0)
            {
                Console.WriteLine("No trips were processed or inserted. Please check your CSV file.");
            }
            else
            {
                Console.WriteLine($"Processing completed successfully. Inserted {insertedTripIds.Count} trips.");
            }
        }

        private void DisplayHighestTipLocation()
        {
            var highestTip = _tripService.GetPULocationWithHighestTip();
            if (highestTip == null)
            {
                Console.WriteLine("No trips are available to calculate the highest average tip.");
            }
            else
            {
                Console.WriteLine($"PULocationID with the highest average tip: {highestTip.PULocationID}, Average Tip: {highestTip.AverageTip}");
            }
        }

        private void DisplayTopTripsByDistance()
        {
            var topTripsByDistance = _tripService.GetTop100LongestTripsByDistance();
            if (topTripsByDistance == null || topTripsByDistance.Count == 0)
            {
                Console.WriteLine("No trips are available to display the longest trips by distance.");
            }
            else
            {
                Console.WriteLine("Top 100 longest trips by distance:");
                foreach (var trip in topTripsByDistance)
                {
                    Console.WriteLine($"TripID: {trip.TripId}, Distance: {trip.TripDistance}");
                }
            }
        }

        private void DisplayTopTripsByDuration()
        {
            var topTripsByDuration = _tripService.GetTop100LongestTripsByDuration();
            if (topTripsByDuration == null || topTripsByDuration.Count == 0)
            {
                Console.WriteLine("No trips are available to display the longest trips by duration.");
            }
            else
            {
                Console.WriteLine("Top 100 longest trips by duration:");
                foreach (var trip in topTripsByDuration)
                {
                    Console.WriteLine($"TripID: {trip.TripId}, Duration: {trip.TripDuration} minutes");
                }
            }
        }

        private void SearchTripsByPULocation()
        {
            Console.Write("Enter the PULocationID to search: ");
            if (int.TryParse(Console.ReadLine(), out var searchPULocationID))
            {
                var tripsByPULocation = _tripService.GetTripsByPULocation(searchPULocationID);
                if (tripsByPULocation == null || tripsByPULocation.Count == 0)
                {
                    Console.WriteLine($"No trips found for PULocationID {searchPULocationID}.");
                }
                else
                {
                    Console.WriteLine($"Trips for PULocationID {searchPULocationID}:");
                    foreach (var trip in tripsByPULocation)
                    {
                        Console.WriteLine($"TripID: {trip.TripId}, PickupDatetime: {trip.PickupDatetime}, DropoffDatetime: {trip.DropoffDatetime}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid PULocationID. Please enter a valid number.");
            }
        }
    }
}
