using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagementModels.ProjectManagement;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels.ProjectManagement
{
    public class ProjectDash
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int TeamID { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int DepartmentID { get; set; }
        public string Keyword { get; set; }
        public List<TeamProj> TeamList { get; set; }
        public List<ProjectList> ProjectList { get; set; }
        public List<SelectListItem> Teams { get; set; }
    }
}
