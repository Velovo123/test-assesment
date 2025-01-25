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
    public class FareRepository
    {
        private readonly IDbConnection _dbConnection;

        public FareRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void InsertFare(Fare fare)
        {
            var query = @"
                INSERT INTO Fare (FareAmount, TipAmount, TripId)
                VALUES (@FareAmount, @TipAmount, @TripId)";

            _dbConnection.Execute(query, fare);
        }

        public IEnumerable<Fare> GetFares()
        {
            var query = "SELECT * FROM Fare";
            return _dbConnection.Query<Fare>(query);
        }

        public Fare? GetFareById(int fareId)
        {
            var query = "SELECT * FROM Fare WHERE FareId = @FareId";
            return _dbConnection.QuerySingleOrDefault<Fare>(query, new { FareId = fareId });
        }

        public void DeleteFareById(int fareId)
        {
            var query = "DELETE FROM Fare WHERE FareId = @FareId";
            _dbConnection.Execute(query, new { FareId = fareId });
        }
    }
}
