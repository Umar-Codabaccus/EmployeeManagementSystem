using Microsoft.Data.SqlClient;
using EmployeeManagementSystem.Models.EmployeeModels.DashboardModels;

namespace EmployeeManagementSystem.DAL.EmployeeDAL
{
    public class EmployeeDashboardDAL
    {
        private Common common;

        public EmployeeDashboardDAL()
        {
            common = new Common();
        }

        public string GetEmployeeName(int employeeID)
        {
            string employeeName = null;

            try
            {
                string query = "SELECT [Firstname], [Lastname] FROM [Employees] " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID",  employeeID);
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return employeeName;
        }
        public int GetTeamID(int employeeID)
        {
            int teamID = 0;

            try
            {
                string query = "SELECT [Teams].[TeamID] " +
                               "FROM [Teams] INNER JOIN [EmployeeTeams] " +
                               "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                               "INNER JOIN [Employees] " +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "WHERE [Employees].[EmployeeID] = @EmployeeID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        teamID = (int)reader["TeamID"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return teamID;
        }
        public string GetTeamName(int teamID)
        {
            string teamName = null;

            try
            {
                string query = "SELECT [TeamName] FROM [Teams] " +
                               "WHERE [TeamID] = @TeamID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        teamName = (string)reader["TeamName"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return teamName;
        }
        public string GetDepartmentName(int teamID)
        {
            string departmentName = null;

            try
            {
                string query = "SELECT [DepartmentName] " +
                               "FROM [Departments] INNER JOIN [Teams] " +
                               "ON [Departments].[DepartmentID] = [Teams].[DepartmentID] " +
                               "WHERE [Teams].[TeamID] = @TeamID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        departmentName = (string)reader["DepartmentName"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return departmentName;
        }
        public List<EmployeeDashboard> GetProjectSpecifications(int employeeID)
        {
            List<EmployeeDashboard> projects = new List<EmployeeDashboard>();

            try
            {
                string query = "SELECT [Description], [ProjectName], [Specifications].[Status], " +
                               "[Specifications].[Deadline] " +
                               "FROM [Specifications] INNER JOIN [Projects] " +
                               "ON [Specifications].[ProjectID] = [Projects].[ProjectID] " +
                               "INNER JOIN [Employees] ON [Employees].[EmployeeID] = [Specifications].[EmployeeID] " +
                               "WHERE [Employees].[EmployeeID] = @EmployeeID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDashboard project = new EmployeeDashboard();
                        project.Specification = (string)reader["Description"];
                        project.ProjectName = (string)reader["ProjectName"];
                        project.Status = (string)reader["Status"];
                        project.Deadline = (DateTime)reader["Deadline"];

                        projects.Add(project);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.GetConnection(); }
            
            return projects;
        }
    }
}
