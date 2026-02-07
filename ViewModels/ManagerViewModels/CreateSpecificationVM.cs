using EmployeeManagementSystem.Models.ManagerModels.Project;

namespace EmployeeManagementSystem.ViewModels.ManagerViewModels
{
    public class CreateSpecificationVM
    {
        public int ProjectID { get; set; }
        public int ManagerID { get; set; }
        public int TeamID { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Keyword { get; set; }
        public int EmployeeID { get; set; }
        public List<SpecificationEmployee> Employees { get; set; }
    }
}
