namespace EmployeeManagementSystem.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int EmployeeID { get; set; }
        public bool LoginSuccessfull { get; set; }
        public bool UsernameEntered { get; set; }
    }
}
