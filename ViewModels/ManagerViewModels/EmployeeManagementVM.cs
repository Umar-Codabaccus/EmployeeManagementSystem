using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.ViewModels.ManagerViewModels
{
    public class EmployeeManagementVM
    {
        public int ManagerID { get; set; }
        public int TeamID { get; set; }
        public string ManagerName { get; set; }
        public List<EmployeeDetails> Employees { get; set; }
    }
}
