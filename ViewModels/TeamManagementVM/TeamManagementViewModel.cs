using EmployeeManagementSystem.Models.ManagementModels.TeamManagement;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.ViewModels.TeamManagementVM
{
    public class TeamManagementViewModel
    {
        public int AdminID { get; set; }
        public int TeamID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeID { get; set; }
        public int PositionID { get; set; }
        public string TeamName { get; set; }
        public string DepartmentName { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public List<TeamSelect> TeamSelectList { get; set; }
        public List<TeamSelect> DepartmentSelectList { get; set; }
        public List<ManagerSelect> ManagerSelectList { get; set; }
        public List<EmployeeSelect> EmployeeSelectList { get; set; }
        public List<SelectListItem> Teams {  get; set; }
        public List<SelectListItem> Managers { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> Departments { get; set; }
    }
}
