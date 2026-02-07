using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public List<PolarChart> PolarChart { get; set; }
        public List<BarChart> BarChart { get; set; }
        public List<ProjectList> Projects { get; set; }
    }
}
