using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels
{
    public class DeleteEmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public string Keyword { get; set; }
        public List<EmployeeDeleteDetails> EmployeeNames { get; set; }
        public List<SelectListItem> FullNames { get; set; }
    }
}
