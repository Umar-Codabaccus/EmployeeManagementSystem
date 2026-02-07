namespace EmployeeManagementSystem.Models
{
    public class EmployeeDash
    {
        public int EmployeeID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string IsManager { get; set; }
        public string Manager {  get; set; }
        public string Fullname { get; set; }
    }
}
