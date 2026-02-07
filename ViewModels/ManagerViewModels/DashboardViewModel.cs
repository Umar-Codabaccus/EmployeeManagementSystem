using EmployeeManagementSystem.Models.ManagerModels.Dashboard;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels.ManagerViewModels
{
    public class DashboardViewModel
    {
        public int UserID { get; set; }
        public int ManagerID { get; set; }
        public string ManagerName { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }
        public List<ManagerDashboard> DashboardList { get; set; }
        public List<ProjectDetails> ProjectList { get; set; }
    }
}
