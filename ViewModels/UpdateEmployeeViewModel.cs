using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels
{
    public class UpdateEmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public string Keyword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Salary { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public List<EmployeeDetails> EmployeeDetails { get; set; }
        public List<SelectListItem> EmployeeNames { get; set; }
        public List<SelectListItem> Positions { get; set; }
    }
}
