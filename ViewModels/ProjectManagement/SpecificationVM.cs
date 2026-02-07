using EmployeeManagementSystem.Models.ManagerModels.Project;

namespace EmployeeManagementSystem.ViewModels.ProjectManagement
{
    public class SpecificationVM
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int ManagerID { get; set; }
        public List<SpecificationManager> Specifications { get; set; }
    }
}
