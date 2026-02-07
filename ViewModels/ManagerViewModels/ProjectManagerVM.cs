using EmployeeManagementSystem.Models.ManagerModels.Project;

namespace EmployeeManagementSystem.ViewModels.ManagerViewModels
{
    public class ProjectManagerVM
    {
        public int ManagerID { get; set; }
        public int TeamID { get; set; }
        public int ProjectID { get; set; }
        public List<ProjectManager> Projects { get; set; }
    }
}
