using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.ViewModels
{
    public class DepartmentViewModel
    {
        public int EmployeeID { get; set; }
        public List<Department> Departments { get; set; }
    }
}
