using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessment.Models;

namespace TestAssessment.Repository
{
    public class ProcessedTripRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProcessedTripRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void InsertProcessedTrip(ProcessedTrip processedTrip)
        {
            var query = @"
                INSERT INTO ProcessedTrip (TripId, IsDuplicate, UTCConversionDatetime)
                VALUES (@TripId, @IsDuplicate, @UTCConversionDatetime)";

            _dbConnection.Execute(query, processedTrip);
        }

        public IEnumerable<ProcessedTrip> GetProcessedTrips()
        {
            var query = "SELECT * FROM ProcessedTrip";
            return _dbConnection.Query<ProcessedTrip>(query);
        }

        public ProcessedTrip? GetProcessedTripById(int processedTripId)
        {
            var query = "SELECT * FROM ProcessedTrip WHERE ProcessedTripId = @ProcessedTripId";
            return _dbConnection.QuerySingleOrDefault<ProcessedTrip>(query, new { ProcessedTripId = processedTripId });
        }

        public void DeleteProcessedTripById(int processedTripId)
        {
            var query = "DELETE FROM ProcessedTrip WHERE ProcessedTripId = @ProcessedTripId";
            _dbConnection.Execute(query, new { ProcessedTripId = processedTripId });
        }
    }
}
