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
    public class LocationRepository
    {
        private readonly IDbConnection _dbConnection;

        public LocationRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void InsertLocation(Location location)
        {
            var query = @"
                INSERT INTO Location (PULocationID, DOLocationID, TripId)
                VALUES (@PULocationID, @DOLocationID, @TripId)";

            _dbConnection.Execute(query, location);
        }

        public IEnumerable<Location> GetLocations()
        {
            var query = "SELECT * FROM Location";
            return _dbConnection.Query<Location>(query);
        }

        public Location? GetLocationById(int locationId)
        {
            var query = "SELECT * FROM Location WHERE LocationId = @LocationId";
            return _dbConnection.QuerySingleOrDefault<Location>(query, new { LocationId = locationId });
        }

        public void DeleteLocationById(int locationId)
        {
            var query = "DELETE FROM Location WHERE LocationId = @LocationId";
            _dbConnection.Execute(query, new { LocationId = locationId });
        }
    }
}
