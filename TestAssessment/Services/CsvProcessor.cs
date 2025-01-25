using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using TestAssessment.Models;

namespace TestAssessment.Services
{
    public class CsvProcessor
    {
        public List<(Trip Trip, Location Location, Fare Fare)> ReadDataFromCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var data = new List<(Trip Trip, Location Location, Fare Fare)>();

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var pickupDatetimeField = csv.GetField("tpep_pickup_datetime");
                var dropoffDatetimeField = csv.GetField("tpep_dropoff_datetime");

                if (string.IsNullOrWhiteSpace(pickupDatetimeField) || string.IsNullOrWhiteSpace(dropoffDatetimeField))
                {
                    throw new InvalidOperationException("Missing datetime fields in the CSV file.");
                }

                var trip = new Trip
                {
                    PickupDatetime = DateTime.ParseExact(
                        pickupDatetimeField.Trim(),
                        "MM/dd/yyyy hh:mm:ss tt",
                        CultureInfo.InvariantCulture
                    ),
                    DropoffDatetime = DateTime.ParseExact(
                        dropoffDatetimeField.Trim(),
                        "MM/dd/yyyy hh:mm:ss tt",
                        CultureInfo.InvariantCulture
                    ),
                    PassengerCount = int.TryParse(csv.GetField("passenger_count")?.Trim(), out var passengerCount) ? passengerCount : 0,
                    TripDistance = double.TryParse(csv.GetField("trip_distance")?.Trim(),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out var tripDistance)
                        ? tripDistance
                        : 0.0,
                    StoreAndFwdFlag = csv.GetField("store_and_fwd_flag")?.Trim() ?? "N"
                };

                var location = new Location
                {
                    PULocationID = int.TryParse(csv.GetField("PULocationID")?.Trim(), out var puLocationId) ? puLocationId : 0,
                    DOLocationID = int.TryParse(csv.GetField("DOLocationID")?.Trim(), out var doLocationId) ? doLocationId : 0,
                    TripId = 0 // Will be set later
                };

                var fare = new Fare
                {
                    FareAmount = decimal.TryParse(csv.GetField("fare_amount")?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var fareAmount)
                        ? fareAmount
                        : 0.0m,
                    TipAmount = decimal.TryParse(csv.GetField("tip_amount")?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var tipAmount)
                        ? tipAmount
                        : 0.0m,
                    TripId = 0 // Will be set later
                };

                data.Add((trip, location, fare));
            }

            return data;
        }
    }
}
