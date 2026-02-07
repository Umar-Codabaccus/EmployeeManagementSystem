using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagementModels.ReportManagement;

namespace EmployeeManagementSystem.ViewModels.ReportManagement
{
    public class EmployeeReportVM
    {
        public int PositionID { get; set; }
        public int DepartmentID { get; set; }
        public string PositionName { get; set; }
        public string DepartmentName { get; set; }
        public string Keyword { get; set; }
        public string Option { get; set; }
        public List<EmployeeModel> Employees { get; set; }
        public List<Department> Departments { get; set; }
        public List<Position> Positions { get; set; }
    }
}
