using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessment.Models;

namespace TestAssessment.Services.IServices
{
    public interface ICsvProcessor
    {
        List<Trip> ReadDataFromCsv(string filePath);
    }
}
