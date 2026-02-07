using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagementModels.TeamManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.ObjectPool;
using System.Data;

namespace EmployeeManagementSystem.DAL
{
    public class AdminManagementDAL
    {
        private Common common;

        public AdminManagementDAL()
        {
            common = new Common();
        }

        public List<TeamSelect> GetTeams()
        {
            List<TeamSelect> teams = new List<TeamSelect>();

            try
            {
                string query = "SELECT [Teams].[TeamID], [TeamName], " +
                               "[Departments].[DepartmentID], [DepartmentName] " +
                               "FROM [Teams] INNER JOIN [Departments] " +
                               "ON [Teams].[DepartmentID] = [Departments].[DepartmentID] " +
                               "WHERE [Teams].[TeamID] NOT IN " +
                               "(SELECT [Teams].[TeamID] " +
                               "FROM [Teams] INNER JOIN [EmployeeTeams] " +
                               "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID])";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TeamSelect team = new TeamSelect();
                        team.TeamID = (int)reader["TeamID"];
                        team.DepartmentID = (int)reader["DepartmentID"];
                        team.TeamName = (string)reader["TeamName"];
                        team.DepartmentName = (string)reader["DepartmentName"];

                        teams.Add(team);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return teams;
        }

        public List<ManagerSelect> GetManagers()
        {
            List<ManagerSelect> managers = new List<ManagerSelect>();

            try
            {
                string query = "SELECT [Employees].[EmployeeID], [FirstName], [LastName], [PositionName] " +
                               "FROM [Employees] INNER JOIN [Users] " +
                               "ON [Employees].[EmployeeID] = [Users].[EmployeeID] " +
                               "INNER JOIN [Roles] " +
                               "ON [Roles].[RoleID] = [Users].[RoleID] " +
                               "INNER JOIN [Positions] " +
                               "ON [Positions].[PositionID] = [Employees].[PositionID] " +
                               "WHERE ([RoleName] = 'Manager' OR [RoleName] = 'Admin') " +
                               "AND [Employees].[EmployeeID] NOT IN " +
                               "(SELECT [Employees].[EmployeeID] " +
                               "FROM [Employees] INNER JOIN [EmployeeTeams] " +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "WHERE [IsManager] = 'yes')";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ManagerSelect manager = new ManagerSelect();
                        manager.EmployeeID = (int)reader["EmployeeID"];
                        manager.FullName = $"{(string)reader["FirstName"]} {(string)reader["LastName"]}";
                        manager.PositionName = (string)reader["PositionName"];

                        managers.Add(manager);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return managers;
        }

        public bool AssignManager(int employeeID, int teamID)
        {
            bool isAssigned = false;
            string isManager = "yes";
            try
            {
                string query = "INSERT INTO [EmployeeTeams] " +
                               "([EmployeeID], [TeamID], [IsManager]) " +
                               "VALUES (@EmployeeID, @TeamID, @IsManager)";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@TeamID", teamID);
                command.Parameters.AddWithValue("@IsManager", isManager);
                common.OpenConnection();
                
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isAssigned = true;
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isAssigned;
        }

        public List<TeamSelect> GetTeamsManagers()
        {
            List<TeamSelect> teams = new List<TeamSelect>();

            try
            {
                string query = "SELECT [Teams].[TeamID], [TeamName], [DepartmentName], [Firstname], [Lastname] " +
                               "FROM [Teams] INNER JOIN [Departments] " +
                               "ON [Teams].[DepartmentID] = [Departments].[DepartmentID] " +
                               "INNER JOIN [EmployeeTeams] " +
                               "ON [EmployeeTeams].[TeamID] = [Teams].[TeamID] " +
                               "INNER JOIN [Employees] " +
                               "ON [EmployeeTeams].[EmployeeID] = [Employees].[EmployeeID] " +
                               "WHERE isManager = 'yes'";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TeamSelect team = new TeamSelect();
                        team.TeamID = (int)reader["TeamID"];
                        team.TeamName = (string)reader["TeamName"];
                        team.DepartmentName = (string)reader["DepartmentName"];
                        team.FullName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";

                        teams.Add(team);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message);  }
            finally { common.CloseConnection(); }

            return teams;
        }

        public List<EmployeeSelect> GetEmployees()
        {
            List<EmployeeSelect> employees = new List<EmployeeSelect>();

            try
            {
                string query = "SELECT [Employees].[EmployeeID], [Firstname], [Lastname], [PositionName] " +
                               "FROM [Employees] INNER JOIN [Positions] " +
                               "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                               "INNER JOIN [Users] ON [Employees].[EmployeeID] = [Users].[EmployeeID] " +
                               "INNER JOIN [Roles] ON [Users].[RoleID] = [Roles].[RoleID] " +
                               "WHERE [RoleName] = 'Employee' " +
                               "AND [Employees].[EmployeeID] NOT IN " +
                               "(SELECT [Employees].[EmployeeID] FROM [Employees] " +
                               "INNER JOIN [EmployeeTeams] " +
                               "ON [EmployeeTeams].[EmployeeID] = [Employees].[EmployeeID] " +
                               "WHERE isManager = 'no')";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeSelect employee = new EmployeeSelect();
                        employee.EmployeeID = (int)reader["EmployeeID"];
                        employee.FullName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                        employee.PositionName = (string)reader["PositionName"];

                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return employees;
        }

        public bool AddEmployeeTeam(int employeeID, int teamID)
        {
            bool isEmployeeAdded = false;
            string isManager = "no";

            try
            {
                string query = "INSERT INTO [EmployeeTeams] ([EmployeeID], [TeamID], [IsManager]) " +
                               "VALUES (@EmployeeID, @TeamID, @IsManager)";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@TeamID", teamID);
                command.Parameters.AddWithValue("@IsManager", isManager);
                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0) { isEmployeeAdded = true; }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isEmployeeAdded;
        }

        public string GetTeamName(int teamID)
        {
            string teamName = "";

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

        public List<TeamSelect> GetTeamsWithNoDept()
        {
            List<TeamSelect> teams = new List<TeamSelect>();

            try
            {
                string query = "SELECT [Teams].[TeamID], [TeamName] " +
                               "FROM [Teams] LEFT JOIN [Departments] " +
                               "ON [Teams].[DepartmentID] = [Departments].[DepartmentID] " +
                               "WHERE [Teams].[TeamID] NOT IN " +
                               "(SELECT [Teams].[TeamID] FROM [Teams] INNER JOIN [Departments] " +
                               "ON [Teams].[DepartmentID] = [Departments].[DepartmentID]);";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TeamSelect team = new TeamSelect();
                        team.TeamID = (int)reader["TeamID"];
                        team.TeamName = (string)reader["TeamName"];

                        teams.Add(team);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return teams;
        }

        public List<TeamSelect> GetDepartments()
        {
            List<TeamSelect> departments = new List<TeamSelect>();

            try
            {
                string query = "SELECT [DepartmentID], [DepartmentName] " +
                               "FROM [Departments] " +
                               "WHERE [DepartmentName] != 'Administrator'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TeamSelect department = new TeamSelect();
                        department.DepartmentID = (int)reader["DepartmentID"];
                        department.DepartmentName = (string)reader["DepartmentName"];

                        departments.Add(department);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return departments;
        }

        public bool AddTeamToDept(int teamID, int departmentID)
        {
            bool isTeamAdded = false;

            try
            {
                string query = "UPDATE [Teams] " +
                               "SET [DepartmentID] = @DepartmentID " +
                               "WHERE [TeamID] = @TeamID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);
                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isTeamAdded = true;
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isTeamAdded;
        }
    }
}
