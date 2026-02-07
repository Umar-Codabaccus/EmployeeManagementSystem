using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagerModels.Dashboard;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagementSystem.DAL.ManagerDAL
{
    public class ManagerDashboardDAL
    {
        private Common common;

        public ManagerDashboardDAL()
        {
            common = new Common();
        }

        public string FindManagerName(int managerID)
        {
            string managerName = " ";

            try
            {
                string query = "SELECT [Firstname], [Lastname] FROM [Employees] WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", managerID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        managerName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return managerName;
        }
        public int FindTeamID(int managerID)
        {
            int teamID = 0;

            try
            {
                string query = "SELECT [Teams].[TeamID] " +
                               "FROM [Teams] INNER JOIN [EmployeeTeams] " +
                               "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                               "INNER JOIN [Employees] ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "WHERE [Employees].[EmployeeID] = @EmployeeID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeId", managerID);
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
        public string FindTeamName(int teamID)
        {
            string teamName = " ";

            try
            {
                string query = "SELECT [TeamName] FROM [Teams] WHERE [TeamID] = @TeamID";
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
        public string FindDepartmentName(int teamID)
        {
            string departmentName = " ";

            try
            {
                string query = "SELECT [DepartmentName] FROM [Departments] " +
                               "INNER JOIN [Teams] ON [Teams].[DepartmentID] = [Departments].[DepartmentID] " +
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
       
        public List<ProjectDetails> GetProjectDetails(int teamID)
        {
            List<ProjectDetails> projects = new List<ProjectDetails>();

            int specificationStatus = 0;
            try
            {
                string query = "SELECT [ProjectName], [Status], [Deadline] " +
                               "FROM [Projects] INNER JOIN [Teams] ON [Projects].[TeamID] = [Teams].[TeamID] " +
                               "WHERE [Teams].[TeamID] = @TeamID";
                
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProjectDetails project = new ProjectDetails();
                        project.ProjectName = (string)reader["ProjectName"];
                        project.Status = (string)reader["Status"];
                        project.Deadline = (DateTime)reader["Deadline"];

                        projects.Add(project);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return projects;
        }

        public List<EmployeeDetails> GetEmployees(int teamID)
        {
            List<EmployeeDetails> employees = new List<EmployeeDetails>();

            try
            {
                string query = "SELECT [Firstname], [Lastname], [PositionName], [Email] " +
                               "FROM [Employees] INNER JOIN [Positions] " +
                               "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                               "INNER JOIN [EmployeeTeams] " +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "WHERE [EmployeeTeams].[TeamID] = @TeamID AND [IsManager] = 'no'";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDetails employee = new EmployeeDetails();
                        employee.FullName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                        employee.PositionName = (string)reader["PositionName"];
                        employee.Email = (string)reader["Email"];

                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return employees;
        }
    }
}
