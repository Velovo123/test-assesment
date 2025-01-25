using Microsoft.Extensions.Configuration;
using TestAssessment.DataAccess;
using TestAssessment.Models;
using TestAssessment.Repository;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbConnection = new DatabaseConnection(configuration);

using var connection = dbConnection.CreateConnection();
var processedTripRepository = new ProcessedTripRepository(connection);

var newProcessedTrip = new ProcessedTrip
{
    TripId = 1,
    IsDuplicate = false,
    UTCConversionDatetime = DateTime.UtcNow
};
processedTripRepository.InsertProcessedTrip(newProcessedTrip);
Console.WriteLine("ProcessedTrip inserted successfully.");

var processedTrips = processedTripRepository.GetProcessedTrips();
foreach (var processedTrip in processedTrips)
{
    Console.WriteLine($"ProcessedTrip ID: {processedTrip.ProcessedTripId}, IsDuplicate: {processedTrip.IsDuplicate}");
}
