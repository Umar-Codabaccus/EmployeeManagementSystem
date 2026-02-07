namespace EmployeeManagementSystem.Models
{
    public class Timesheet
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Workdate { get; set; }
        public TimeSpan ClockIn { get; set; }
        public TimeSpan ClockOut { get; set; }
        public double HoursWorked { get; set; }
        public double Overtime { get; set; }
    }
}
