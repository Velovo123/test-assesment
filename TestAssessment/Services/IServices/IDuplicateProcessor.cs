using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessment.Models;

namespace TestAssessment.Services.IServices
{
    public interface IDuplicateProcessor
    {
        (List<Trip> UniqueTrips, List<Trip> Duplicates) FindAndRemoveDuplicates(List<Trip> trips, string duplicatesFilePath);
    }
}
