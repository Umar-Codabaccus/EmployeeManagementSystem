namespace EmployeeManagementSystem.Models
{
    public class Team
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int NumberOfEmployees { get; set; }
    }
}
