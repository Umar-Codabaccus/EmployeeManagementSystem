using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL
{
    public class AdminEmployeeDAL
    {
        private Common common;

        public AdminEmployeeDAL()
        {
            common = new Common();
        }

        public List<EmployeeDash> GetEmployeeDash()
        {
            List<EmployeeDash> employees = new List<EmployeeDash>();
            string manager;
            try
            {
                string query = "SELECT [Employees].[Firstname], [Employees].[Lastname], " +
                               "[Teams].[TeamID], [TeamName], " +
                               "[DepartmentName], [IsManager] " +
                               "FROM [Employees] " +
                               "LEFT JOIN [EmployeeTeams] " +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "LEFT JOIN [Teams] " +
                               "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                               "LEFT JOIN [Departments] " +
                               "ON [Departments].[DepartmentID] = [Teams].[DepartmentID]";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDash employeeDash = new EmployeeDash();
                        employeeDash.Firstname = (string)reader["Firstname"];
                        employeeDash.Lastname = (string)reader["Lastname"];
                        employeeDash.TeamID = reader["TeamID"] == DBNull.Value ? 0 : (int)reader["TeamID"];
                        employeeDash.TeamName = reader["TeamName"] == DBNull.Value ? null : (string)reader["TeamName"];
                        employeeDash.DepartmentName = reader["DepartmentName"] == DBNull.Value ? null : (string)reader["DepartmentName"];
                        employeeDash.IsManager = reader["IsManager"] == DBNull.Value ? null : (string)reader["IsManager"];
                        employeeDash.Fullname = employeeDash.Firstname + " " + employeeDash.Lastname;

                        employees.Add(employeeDash);
                     }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                common.CloseConnection();
            }

            return employees;
        }

        public string GetManager(int teamID)
        {
            string manager = null;

            try
            {
                string query = "SELECT [Firstname], [Lastname]" +
                               "FROM [Employees] INNER JOIN [EmployeeTeams]" +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID]" +
                               "INNER JOIN [Teams] ON [Teams].[TeamID] = [EmployeeTeams].[TeamID]" +
                               "WHERE [Teams].[TeamID] = @TeamID AND [IsManager] = 'yes';";
                SqlCommand command = new SqlCommand(@query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        manager = (string)reader["Firstname"] + " " + (string)reader["Lastname"];
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                common.CloseConnection();
            }

            return manager;
        }

        public bool AddEmployee(string firstName, string lastName, string email,
            int phone, string city, string street, DateTime dob, int salary, int positionID)
        {
            bool isEmployeeAdded = false;
            try
            {
                string query = "INSERT INTO [Employees]" +
                               "([FirstName], [LastName], [DateOfBirth], [Email], [Phone]," +
                               "[City], [Street], [Salary], [PositionID])" +
                               "VALUES" +
                               "(@FirstName, @LastName, @DateOfBirth, @Email, @Phone, @City, @Street," +
                               "@Salary, @PositionID)";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@DateOfBirth", dob.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@Street", street);
                command.Parameters.AddWithValue("@Salary", salary);
                command.Parameters.AddWithValue("@PositionID", positionID);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isEmployeeAdded = true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception (ex.Message);
            }
            finally
            {
                common.CloseConnection();
            }

            return isEmployeeAdded;
        }

        public bool CreateUser(string username, string password, int employeeID, int roleID)
        {
            bool IsCreated = false;

            try
            {
                string query = "INSERT INTO [Users] ([Username], [Password], [EmployeeID], [RoleID])" +
                               "VALUES (@Username, @Password, @EmployeeID, @RoleID)";
                SqlCommand command = new SqlCommand (query, common.GetConnection());
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@RoleID", roleID);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    IsCreated = true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                common.CloseConnection();
            }
            return IsCreated;
        }

        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();

            try
            {
                string query = "SELECT [RoleID], [RoleName] FROM [Roles]";
                SqlCommand command = new SqlCommand(query, common.GetConnection());

                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Role role = new Role();
                        int roleID = (int)reader["RoleID"];
                        string roleName = (string)reader["RoleName"];

                        if (roleName.ToLower() != "admin")
                        {
                            role.RoleID = roleID;
                            role.RoleName = roleName;
                        }

                        roles.Add(role);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception (ex.Message);
            }
            finally
            {
                common.CloseConnection();
            }

            return roles;
        }

        public int GetEmployeeID(string email)
        {
            int empID = 0;
            try
            {
                string query = "SELECT [EmployeeID] FROM [Employees]" +
                               "WHERE [Email] = @Email";
                SqlCommand command = new SqlCommand (query, common.GetConnection());
                command.Parameters.AddWithValue("@Email", email);
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        empID = (int)reader["EmployeeID"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
            finally { common.CloseConnection(); }

            return empID;
        }

        public bool CheckEmailUniqueness(string email)
        {
            bool isUnique = true;
            try
            {
                string query = "SELECT [Email] FROM [Employees]" +
                               "WHERE [Email] = @Email";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@Email", email);
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isUnique = false;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
                isUnique = false;
            }
            finally { common.CloseConnection(); }

            return isUnique;
        }

        public List<EmployeeViewAll> GetEmployees()
        {
            List<EmployeeViewAll> employees = new List<EmployeeViewAll>();

            try
            {
                string query = "SELECT [FirstName], [LastName], [DateOfBirth], [Email], [Phone]," +
                               "[City], [Street], [PositionName], [Salary]" +
                               "FROM [Employees] LEFT JOIN [Positions]" +
                               "ON [Employees].[PositionID] = [Positions].[PositionID]";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeViewAll emp = new EmployeeViewAll();
                        emp.FirstName = (string)reader["FirstName"];
                        emp.LastName = (string)reader["LastName"];
                        emp.DateOfBirth = (DateTime)reader["DateOfBirth"];
                        emp.Email = (string)reader["Email"];
                        emp.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
                        emp.City = reader["City"] == DBNull.Value ? null : (string)reader["City"];
                        emp.Street = reader["Street"] == DBNull.Value ? null : (string)reader["Street"];
                        emp.PositionName = reader["PositionName"] == DBNull.Value ? null : (string)reader["PositionName"];
                        emp.Salary = reader["Salary"] == DBNull.Value ? 0 : (int)reader["Salary"];

                        emp.DisplaySalary = $"Rs {emp.Salary}";
                        emp.FullName = emp.FirstName + " " + emp.LastName;

                        employees.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return employees;
        }

        public List<EmployeeDeleteDetails> GetEmployeeNames()
        {
            List<EmployeeDeleteDetails> employeeDeleteDetails = new List<EmployeeDeleteDetails>();

            try
            {
                string query = "SELECT [Employees].[EmployeeID], [FirstName], [LastName], [Roles].[RoleID] FROM [Employees] INNER JOIN [Users]" +
                               "ON [Employees].[EmployeeID] = [Users].[EmployeeID]" +
                               "INNER JOIN [Roles] ON [Users].[RoleID] = [Roles].[RoleID]" +
                               "WHERE [RoleName] NOT LIKE 'Admin'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDeleteDetails emp = new EmployeeDeleteDetails();
                        emp.EmployeeID = (int)reader["EmployeeID"];
                        emp.FullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}";
                        emp.RoleID = (int)reader["RoleID"];
                        employeeDeleteDetails.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception (ex.Message);
            }
            finally { common.CloseConnection(); }

            return employeeDeleteDetails;
        }

        public List<EmployeeDeleteDetails> GetEmployeeByKeyword(string keyword)
        {
            List<EmployeeDeleteDetails> employees = new List<EmployeeDeleteDetails>();

            try
            {
                string query = $"SELECT [Employees].[EmployeeID], [FirstName], [LastName]" +
                               $"FROM [Employees] INNER JOIN [Users]" +
                               $"ON [Employees].[EmployeeID] = [Users].[EmployeeID]" +
                               $"INNER JOIN [Roles]" +
                               $"ON [Users].[RoleID] = [Roles].[RoleID]" +
                               $"WHERE ([FirstName] LIKE '%{keyword}%' OR [LastName] LIKE '%{keyword}%')" +
                               $"AND [RoleName] NOT LIKE 'Admin'";
                SqlCommand command = new SqlCommand (query, common.GetConnection());
                
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDeleteDetails emp = new EmployeeDeleteDetails();
                        emp.EmployeeID = (int)reader["EmployeeID"];
                        emp.FullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}";
                        //emp.FullName = (string)reader["[FirstName] + ' ' + [LastName]"];
                        employees.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return employees;
        }

        public bool DeleteEmployee(int employeeID)
        {
            bool isDeleted = false;

            try
            {
                string query = "DELETE FROM [Employees] WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isDeleted = true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception (ex.Message);
            }
            finally { common.CloseConnection(); }

            return isDeleted;
        }
        public List<EmployeeDetails> GetEmployeeByKeywordForUpdate(string keyword)
        {
            List<EmployeeDetails> employees = new List<EmployeeDetails>();

            try
            {
                string query = $"SELECT [Employees].[EmployeeID], [FirstName], [LastName], [RoleName]" +
                               $"FROM [Employees] INNER JOIN [Users]" +
                               $"ON [Employees].[EmployeeID] = [Users].[EmployeeID]" +
                               $"INNER JOIN [Roles]" +
                               $"ON [Users].[RoleID] = [Roles].[RoleID]" +
                               $"WHERE ([FirstName] LIKE '%{keyword}%' OR [LastName] LIKE '%{keyword}%')" +
                               $"AND [RoleName] NOT LIKE 'Admin'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDetails emp = new EmployeeDetails();
                        emp.EmployeeID = (int)reader["EmployeeID"];
                        emp.FullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}";
                        emp.RoleName = (string)reader["RoleName"];
                        //emp.FullName = (string)reader["[FirstName] + ' ' + [LastName]"];
                        employees.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return employees;
        }

        public string GetEmployeeName(int employeeID)
        {
            string fullName = "";

            try
            {
                string query = "SELECT [FirstName], [LastName] FROM [Employees]" +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand (query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        fullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}";
                    }
                }
            }
            catch (SqlException ex)
            {
                fullName = ex.Message;
            }
            finally { common.CloseConnection(); }

            return fullName;
        }
    }
}
