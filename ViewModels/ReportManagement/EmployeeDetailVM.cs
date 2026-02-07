using EmployeeManagementSystem.Models.ManagementModels;

namespace EmployeeManagementSystem.ViewModels.ReportManagement
{
    public class EmployeeDetailVM
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Salary { get; set; }
        public string PositionName { get; set; }
        public string TeamName { get; set; }
        public string DepartmentName { get; set; }
        public string Keyword { get; set; }
        public List<EmployeeDetail> Employees { get; set; }
    }
}
