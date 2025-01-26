using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using TestAssessment.DataAccess;
using TestAssessment.Repository;
using TestAssessment.Repository.IRepository;
using TestAssessment.Services;
using TestAssessment.Services.IServices;
using TestAssessment.UI;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)

    .AddScoped<DatabaseConnection>()
    .AddScoped<IDbConnection>(provider =>
        provider.GetRequiredService<DatabaseConnection>().CreateConnection())

    .AddScoped<ITripRepository, TripRepository>()
    .AddScoped<ITripService, TripService>()
    .AddScoped<ICsvProcessor, CsvProcessor>()
    .AddScoped<IDuplicateProcessor, DuplicateProcessor>()

    .BuildServiceProvider();

var tripService = serviceProvider.GetRequiredService<ITripService>();

try
{
    var tripMenu = new TripMenu(tripService);
    tripMenu.DisplayMenu();
}
catch
{
    Console.WriteLine("An unexpected error occurred. Please contact support.");
}
