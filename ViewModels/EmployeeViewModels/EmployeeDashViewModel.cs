using EmployeeManagementSystem.Models.EmployeeModels.DashboardModels;

namespace EmployeeManagementSystem.ViewModels.EmployeeViewModels
{
    public class EmployeeDashViewModel
    {
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string DepartmentName { get; set; }
        public List<EmployeeDashboard> ProjectList { get; set; }
    }
}
