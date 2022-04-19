using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }


    }
}