using EmployeeManagementSystem.Models.EmployeeModels.DashboardModels;

namespace EmployeeManagementSystem.ViewModels.EmployeeViewModels
{
    public class EmployeeProjectVM
    {
        public int EmployeeID { get; set; }
        public int TeamID { get; set; }
        public int SpecificationID { get; set; }
        public List<ProjectDashboard> Projects { get; set; }
    }
}
