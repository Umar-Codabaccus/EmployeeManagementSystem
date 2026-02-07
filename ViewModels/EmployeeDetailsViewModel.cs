using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Salary { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<SelectListItem> Fullnames { get; set; }
        public List<EmployeeDetails> EmployeeDetailsList { get; set; }
        public List<Position> PositionList { get; set; }
        public List<SelectListItem> Positions { get; set; }
        public List<Role> RoleList { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
