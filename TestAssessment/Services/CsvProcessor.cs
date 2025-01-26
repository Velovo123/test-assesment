using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using TestAssessment.Models;
using TestAssessment.Services.IServices;

namespace TestAssessment.Services
{
    public class CsvProcessor : ICsvProcessor
    {
        public List<Trip> ReadDataFromCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var unifiedTrips = new List<Trip>();

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

                var unifiedTrip = new Trip
                {
                    PickupDatetime = ConvertToUtc(
                        pickupDatetimeField.Trim(),
                        "MM/dd/yyyy hh:mm:ss tt"
                    ),
                    DropoffDatetime = ConvertToUtc(
                        dropoffDatetimeField.Trim(),
                        "MM/dd/yyyy hh:mm:ss tt"
                    ),
                    PassengerCount = int.TryParse(csv.GetField("passenger_count")?.Trim(), out var passengerCount) ? passengerCount : 0,
                    TripDistance = double.TryParse(csv.GetField("trip_distance")?.Trim(),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out var tripDistance)
                        ? tripDistance
                        : 0.0,
                    StoreAndFwdFlag = NormalizeStoreAndFwdFlag(csv.GetField("store_and_fwd_flag")?.Trim()),

                    PULocationID = int.TryParse(csv.GetField("PULocationID")?.Trim(), out var puLocationId) ? puLocationId : 0,
                    DOLocationID = int.TryParse(csv.GetField("DOLocationID")?.Trim(), out var doLocationId) ? doLocationId : 0,

                    FareAmount = decimal.TryParse(csv.GetField("fare_amount")?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var fareAmount)
                        ? fareAmount
                        : 0.0m,
                    TipAmount = decimal.TryParse(csv.GetField("tip_amount")?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var tipAmount)
                        ? tipAmount
                        : 0.0m
                };

                unifiedTrips.Add(unifiedTrip);
            }

            return unifiedTrips;
        }

        private static DateTime ConvertToUtc(string dateTimeString, string format)
        {
            var estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            var localTime = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);

            return TimeZoneInfo.ConvertTimeToUtc(localTime, estZone);
        }

        private static string NormalizeStoreAndFwdFlag(string? flag)
        {
            if (string.IsNullOrWhiteSpace(flag)) return "No";
            return flag.ToUpper() switch
            {
                "Y" => "Yes",
                "N" => "No",
                _ => flag
            };
        }
    }
}
