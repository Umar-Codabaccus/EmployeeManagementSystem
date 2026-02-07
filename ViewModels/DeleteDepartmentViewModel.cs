using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels
{
    public class DeleteDepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public List<Department> DepartmentList {  get; set; }
        public List<SelectListItem> Departments { get; set; }
    }
}
