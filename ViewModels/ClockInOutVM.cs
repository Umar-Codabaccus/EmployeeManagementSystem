using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.ViewModels
{
    public class ClockInOutVM
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int TeamID { get; set; }
        public DateTime Workdate { get; set; }
        public string DisplayWorkdate { get; set; }
        public string FormattedTime { get; set; }
        public TimeSpan ClockIn { get; set; }
        public TimeSpan ClockOut { get; set; }
        public List<Timesheet> Attendances { get; set; }
    }
}
