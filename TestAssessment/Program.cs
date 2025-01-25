using Microsoft.Extensions.Configuration;
using TestAssessment.DataAccess;
using TestAssessment.Models;
using TestAssessment.Repository;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbConnection = new DatabaseConnection(configuration);

using var connection = dbConnection.CreateConnection();
var tripRepository = new TripRepository(connection);

var newTrip = new Trip(DateTime.Now, DateTime.Now.AddHours(1), 3, 12.5, "N");
tripRepository.InsertTrip(newTrip);
Console.WriteLine("Trip inserted successfully.");


var trips = tripRepository.GetTrips();
foreach (var trip in trips)
{
    Console.WriteLine($"Trip ID: {trip.TripId}, Distance: {trip.TripDistance}");
}

