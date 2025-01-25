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
    public class TripRepository
    {
        private readonly IDbConnection _dbConnection;

        public TripRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void InsertTrip(Trip trip)
        {
            var query = @"
                INSERT INTO Trip (PickupDatetime, DropoffDatetime, PassengerCount, TripDistance, StoreAndFwdFlag)
                VALUES (@PickupDatetime, @DropoffDatetime, @PassengerCount, @TripDistance, @StoreAndFwdFlag)";

            _dbConnection.Execute(query, trip);
        }

        public IEnumerable<Trip> GetTrips()
        {
            var query = "SELECT * FROM Trip";
            return _dbConnection.Query<Trip>(query);
        }

        public Trip? GetTripById(int tripId)
        {
            var query = "SELECT * FROM Trip WHERE TripId = @TripId";
            return _dbConnection.QuerySingleOrDefault<Trip>(query, new { TripId = tripId });
        }

        public void DeleteTripById(int tripId)
        {
            var query = "DELETE FROM Trip WHERE TripId = @TripId";
            _dbConnection.Execute(query, new { TripId = tripId });
        }
    }
}
