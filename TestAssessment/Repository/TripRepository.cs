using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestAssessment.Models;
using TestAssessment.Repository.IRepository;

namespace TestAssessment.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly IDbConnection _dbConnection;

        public TripRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int InsertUnifiedTrip(Trip trip)
        {
            var query = @"
                INSERT INTO Trips (PickupDatetime, DropoffDatetime, PassengerCount, TripDistance, StoreAndFwdFlag, 
                                         PULocationID, DOLocationID, FareAmount, TipAmount)
                VALUES (@PickupDatetime, @DropoffDatetime, @PassengerCount, @TripDistance, @StoreAndFwdFlag, 
                        @PULocationID, @DOLocationID, @FareAmount, @TipAmount);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return _dbConnection.QuerySingle<int>(query, trip);
        }

        public IEnumerable<Trip> GetUnifiedTrips()
        {
            var query = "SELECT * FROM Trips";
            return _dbConnection.Query<Trip>(query);
        }

        public Trip? GetUnifiedTripById(int tripId)
        {
            var query = "SELECT * FROM Trips WHERE TripId = @TripId";
            return _dbConnection.QuerySingleOrDefault<Trip>(query, new { TripId = tripId });
        }

        public void DeleteUnifiedTripById(int tripId)
        {
            var query = "DELETE FROM Trips WHERE TripId = @TripId";
            _dbConnection.Execute(query, new { TripId = tripId });
        }

        public void BulkInsertUnifiedTrips(IEnumerable<Trip> trips, out List<int> generatedIds)
        {
            var connectionString = _dbConnection.ConnectionString;
            generatedIds = new List<int>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                var tempTableName = "#TempUnifiedTrips";

                var createTableQuery = $@"
                    CREATE TABLE {tempTableName} (
                        PickupDatetime DATETIME,
                        DropoffDatetime DATETIME,
                        PassengerCount INT,
                        TripDistance FLOAT,
                        StoreAndFwdFlag NVARCHAR(3),
                        PULocationID INT,
                        DOLocationID INT,
                        FareAmount DECIMAL(18, 2),
                        TipAmount DECIMAL(18, 2)
                    );";
                using (var command = new SqlCommand(createTableQuery, sqlConnection))
                {
                    command.ExecuteNonQuery();
                }

                try
                {
                    using var dataTable = new DataTable();
                    dataTable.Columns.Add("PickupDatetime", typeof(DateTime));
                    dataTable.Columns.Add("DropoffDatetime", typeof(DateTime));
                    dataTable.Columns.Add("PassengerCount", typeof(int));
                    dataTable.Columns.Add("TripDistance", typeof(double));
                    dataTable.Columns.Add("StoreAndFwdFlag", typeof(string));
                    dataTable.Columns.Add("PULocationID", typeof(int));
                    dataTable.Columns.Add("DOLocationID", typeof(int));
                    dataTable.Columns.Add("FareAmount", typeof(decimal));
                    dataTable.Columns.Add("TipAmount", typeof(decimal));

                    foreach (var trip in trips)
                    {
                        dataTable.Rows.Add(
                            trip.PickupDatetime,
                            trip.DropoffDatetime,
                            trip.PassengerCount,
                            trip.TripDistance,
                            trip.StoreAndFwdFlag,
                            trip.PULocationID,
                            trip.DOLocationID,
                            trip.FareAmount,
                            trip.TipAmount
                        );
                    }

                    using (var bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = tempTableName;
                        bulkCopy.WriteToServer(dataTable);
                    }

                    var insertQuery = $@"
                        INSERT INTO Trips (PickupDatetime, DropoffDatetime, PassengerCount, TripDistance, StoreAndFwdFlag, 
                                                 PULocationID, DOLocationID, FareAmount, TipAmount)
                        OUTPUT INSERTED.TripId
                        SELECT PickupDatetime, DropoffDatetime, PassengerCount, TripDistance, StoreAndFwdFlag, 
                               PULocationID, DOLocationID, FareAmount, TipAmount
                        FROM {tempTableName};";

                    using (var command = new SqlCommand(insertQuery, sqlConnection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            generatedIds.Add(reader.GetInt32(0));
                        }
                    }
                }
                finally
                {
                    var dropTableQuery = $"DROP TABLE {tempTableName};";
                    using (var command = new SqlCommand(dropTableQuery, sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            
        }
        public PULocationIDWithHighestTip GetPULocationWithHighestTip()
        {
            var query = @"
            SELECT TOP 1 
                PULocationID,
                AVG(TipAmount) AS AverageTip
            FROM Trips
            GROUP BY PULocationID
            ORDER BY AverageTip DESC;";

            return _dbConnection.QuerySingle<PULocationIDWithHighestTip>(query);
        }

        public List<Trip> GetTop100LongestTripsByDistance()
        {
            var query = @"
            SELECT TOP 100 
                *
            FROM Trips
            ORDER BY TripDistance DESC;";

            return _dbConnection.Query<Trip>(query).ToList();
        }

        public List<TripWithDuration> GetTop100LongestTripsByDuration()
        {
            var query = @"
                SELECT TOP 100 
                    *,
                    DATEDIFF(MINUTE, PickupDatetime, DropoffDatetime) AS TripDuration
                FROM Trips
                ORDER BY TripDuration DESC;";

            return _dbConnection.Query<TripWithDuration>(query).ToList();
        }

        public List<Trip> GetTripsByPULocation(int puLocationId)
        {
            var query = @"
            SELECT 
                *
            FROM Trips
            WHERE PULocationID = @PULocationID;";

            return _dbConnection.Query<Trip>(query, new { PULocationID = puLocationId }).ToList();
        }
    }
}
