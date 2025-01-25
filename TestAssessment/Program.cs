using Microsoft.Extensions.Configuration;
using TestAssessment.DataAccess;
using TestAssessment.Models;
using TestAssessment.Repository;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbConnection = new DatabaseConnection(configuration);

using var connection = dbConnection.CreateConnection();
var fareRepository = new FareRepository(connection);

var newFare = new Fare
{
    FareAmount = 25.50m,
    TipAmount = 5.00m,
    TripId = 1
};
fareRepository.InsertFare(newFare);
Console.WriteLine("Fare inserted successfully.");

var fares = fareRepository.GetFares();
foreach (var fare in fares)
{
    Console.WriteLine($"Fare ID: {fare.FareId}, Amount: {fare.FareAmount}, Tip: {fare.TipAmount}");
}
