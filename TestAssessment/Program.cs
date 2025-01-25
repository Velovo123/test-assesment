using Microsoft.Extensions.Configuration;
using TestAssessment.DataAccess;
using TestAssessment.Models;
using TestAssessment.Repository;
using TestAssessment.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbConnection = new DatabaseConnection(configuration);

using var connection = dbConnection.CreateConnection();

var tripRepository = new TripRepository(connection);
var locationRepository = new LocationRepository(connection);
var fareRepository = new FareRepository(connection);

var csvProcessor = new CsvProcessor();

var filePath = "sample-cab-data.csv"; 
var data = csvProcessor.ReadDataFromCsv(filePath);

foreach (var (trip, location, fare) in data)
{
    // Insert Trip and get TripId
    var tripId = tripRepository.InsertTrip(trip);

    // Associate TripId with Location and insert
    location.TripId = tripId;
    locationRepository.InsertLocation(location);

    // Associate TripId with Fare and insert
    fare.TripId = tripId;
    fareRepository.InsertFare(fare);
}

Console.WriteLine("Data processed and inserted successfully.");
