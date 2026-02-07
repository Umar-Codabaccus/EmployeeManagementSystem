using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.ViewModels
{
    public class EmployeeAllViewModel
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PositionName { get; set; }
        public int Salary { get; set; }
        public string DisplaySalary { get; set; }
        public List<EmployeeViewAll> Employees { get; set; }
    }
}
