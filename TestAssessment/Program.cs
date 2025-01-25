using Microsoft.Extensions.Configuration;
using TestAssessment.DataAccess;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbConnection = new DatabaseConnection(configuration);

using var connection = dbConnection.CreateConnection();

Console.WriteLine("Database connection initialized successfully.");