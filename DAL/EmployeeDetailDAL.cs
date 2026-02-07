using EmployeeManagementSystem.Models.ManagementModels;
using EmployeeManagementSystem.ViewModels.ReportManagement;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL
{
    public class EmployeeDetailDAL
    {
        private Common common;

        public EmployeeDetailDAL()
        {
            common = new Common();
        }

        public List<EmployeeDetail> GetEmployees(string keyword)
        {
            List<EmployeeDetail> employees = new List<EmployeeDetail>();

            try
            {
                string query = $"SELECT [EmployeeID], [Firstname], [Lastname] " +
                               $"FROM [Employees] " +
                               $"WHERE [Firstname] LIKE '%{keyword}%' OR [Lastname] LIKE '%{keyword}%'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDetail employee = new EmployeeDetail();
                        employee.EmployeeID = (int)reader["EmployeeID"];
                        employee.EmployeeName = (string)reader["Firstname"] + " " + (string)reader["Lastname"];

                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return employees;
        }

        public EmployeeDetailVM GetEmployee(int employeeID)
        {
            EmployeeDetailVM employee = new EmployeeDetailVM();

            try
            {
                string query = "SELECT [Firstname], [Lastname], [DateOfBirth], [Email], [Phone], [City], " +
                               "[Street], [Salary], [PositionName], [TeamName], [DepartmentName] " +
                               "FROM [Employees] INNER JOIN [Positions] " +
                               "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                               "INNER JOIN [EmployeeTeams] " +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "INNER JOIN [Teams] " +
                               "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                               "INNER JOIN [Departments] " +
                               "ON [Departments].[DepartmentID] = [Teams].[DepartmentID] " +
                               "WHERE [Employees].[EmployeeID] = @EmployeeID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                        employee.DateOfBirth = (DateTime)reader["DateOfBirth"];
                        employee.Email = (string)reader["Email"];
                        employee.Phone = (string)reader["Phone"];
                        employee.Address = $"{(string)reader["City"]}, {(string)reader["Street"]}";
                        employee.Salary = (int)reader["Salary"];
                        employee.PositionName = (string)reader["PositionName"];
                        employee.TeamName = (string)reader["TeamName"];
                        employee.DepartmentName = (string)reader["DepartmentName"];
                    }
                }
            }
            catch(SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return employee;
        }
    }
}
