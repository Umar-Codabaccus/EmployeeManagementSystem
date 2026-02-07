using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.ViewModels
{
    public class EmployeeViewModel
    {
        public int AdminID { get; set; }
        public int TeamID { get; set; }
        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public string Manager { get; set; }
        public List<EmployeeDash> EmployeeList { get; set; }
    }
}
