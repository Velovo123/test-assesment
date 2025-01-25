using Microsoft.Extensions.Configuration;
using TestAssessment.DataAccess;
using TestAssessment.Models;
using TestAssessment.Repository;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbConnection = new DatabaseConnection(configuration);

using var connection = dbConnection.CreateConnection();
var locationRepository = new LocationRepository(connection);

var newLocation = new Location
{
    PULocationID = 1,
    DOLocationID = 2,
    TripId = 1
};
locationRepository.InsertLocation(newLocation);
Console.WriteLine("Location inserted successfully.");

var locations = locationRepository.GetLocations();
foreach (var location in locations)
{
    Console.WriteLine($"Location ID: {location.LocationId}, PU: {location.PULocationID}, DO: {location.DOLocationID}");
}
