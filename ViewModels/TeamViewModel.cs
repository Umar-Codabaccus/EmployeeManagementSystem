using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels
{
    public class TeamViewModel
    {
        public int EmployeeID { get; set; }
        public int TeamID { get; set; }
        public string? TeamName { get; set; }
        public int DepartmentID { get; set; }
        public int NumberOfEmployees { get; set; }
        public List<Team> TeamList { get; set; }
        public List<SelectListItem> Teams { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<SelectListItem> Departments { get; set; }
    }
}
