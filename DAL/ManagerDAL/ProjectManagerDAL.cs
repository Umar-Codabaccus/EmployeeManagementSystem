using EmployeeManagementSystem.Models.ManagerModels.Project;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace EmployeeManagementSystem.DAL.ManagerDAL
{
    public class ProjectManagerDAL
    {
        private Common common;

        public ProjectManagerDAL()
        {
            common = new Common();
        }

        public List<ProjectManager> GetProject(int teamID)
        {
            List<ProjectManager> projects = new List<ProjectManager>();

            try
            {
                string query = "SELECT [Projects].[ProjectID], [ProjectName] " +
                               "FROM [Projects] INNER JOIN [Teams] " +
                               "ON [Projects].[TeamID] = [Teams].[TeamID] " +
                               "WHERE [Teams].[TeamID] = @TeamID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProjectManager project = new ProjectManager();
                        project.ProjectID = (int)reader["ProjectID"];
                        project.ProjectName = (string)reader["ProjectName"];

                        projects.Add(project);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return projects;
        }

        public string GetProjectName(int projectID)
        {
            string projectName = "";

            try
            {
                string query = "SELECT [ProjectName] FROM [Projects] WHERE [ProjectID] = @ProjectID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectID", projectID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        projectName = (string)reader["ProjectName"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return projectName;
        }

        public List<SpecificationManager> GetSpecifications(int projectID)
        {
            List<SpecificationManager> specifications = new List<SpecificationManager>();

            try
            {
                string query = "SELECT [Description], [Specifications].[Status], [DateAssigned], " +
                               "[Specifications].[Deadline], " +
                               "[Firstname], [Lastname] " +
                               "FROM [Specifications] INNER JOIN [Employees] " +
                               "ON [Specifications].[EmployeeID] = [Employees].[EmployeeID] " +
                               "INNER JOIN [Projects] ON [Projects].[ProjectID] = [Specifications].[ProjectID] " +
                               "WHERE [Projects].[ProjectID] = @ProjectID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectID", projectID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SpecificationManager specification = new SpecificationManager();
                        specification.Description = (string)reader["Description"];
                        specification.Status = (string)reader["Status"];
                        specification.DateAssigned = (DateTime)reader["DateAssigned"];
                        specification.Deadline = (DateTime)reader["Deadline"];
                        specification.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";

                        specifications.Add(specification);

                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return specifications;
        }

        public int SpecificationCount(int projectID)
        {
            int count = 0;
            try
            {
                string query = "SELECT COUNT(*) AS NumSpecifications " +
                               "FROM [Specifications] INNER JOIN [Projects] " +
                               "ON [Specifications].[ProjectID] = [Projects].[ProjectID] " +
                               "WHERE [Projects].[ProjectID] = @ProjectID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectID", projectID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader["NumSpecifications"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return count;
        }

        public bool CheckProjectStatus(int projectID, int numSpecifications)
        {
            bool isCompleted = false;
            int count = 0;
            try
            {
                string query = "SELECT COUNT(*) AS NumSpecificationsCompleted " +
                               "FROM [Specifications] INNER JOIN [Projects] " +
                               "ON [Specifications].[ProjectID] = [Projects].[ProjectID] " +
                               "WHERE [Specifications].[Status] = 'Completed' " +
                               "AND [Projects].[ProjectID] = @ProjectID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectID", projectID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = (int)reader["NumSpecificationsCompleted"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            if (count == numSpecifications)
            {
                isCompleted = true;
            }

            return isCompleted;
        }

        public List<SpecificationEmployee> GetEmployees(int teamID, string keyword)
        {
            List<SpecificationEmployee> employees = new List<SpecificationEmployee>();

            try
            {
                string query = $"SELECT [Employees].[EmployeeID], [Firstname], [Lastname], [PositionName] " +
                               $"FROM [Employees] INNER JOIN [Positions] " +
                               $"ON [Employees].[PositionID] = [Positions].[PositionID] " +
                               $"INNER JOIN [EmployeeTeams] " +
                               $"ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               $"INNER JOIN [Teams] ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                               $"WHERE [Teams].[TeamID] = @TeamID " +
                               $"AND ([Firstname] LIKE '%{keyword}%' OR [Lastname] LIKE '%{keyword}%');";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@TeamID", teamID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SpecificationEmployee employee = new SpecificationEmployee();
                        employee.EmployeeID = (int)reader["EmployeeID"];
                        employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                        employee.PositionName = (string)reader["PositionName"];

                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return employees;
        }

        public bool CreateSpecification(int projectID, int employeeID, string description, DateTime deadline)
        {
            bool isCreated = false;
            string status = "In Progress";
            DateTime dateAssigned = DateTime.Now;
            
            try
            {
                string query = "INSERT INTO [Specifications] " +
                               "([ProjectID], [EmployeeID], [Description], [Status], [DateAssigned], [Deadline]) " +
                               "VALUES (@ProjectID, @EmployeeID, @Description, @Status, @DateAssigned, @Deadline)";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectID", projectID);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@DateAssigned", dateAssigned);
                command.Parameters.AddWithValue("@Deadline", deadline);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isCreated = true;
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isCreated;
        }

        public bool UpdateProjectStatus(int projectID)
        {
            bool isUpdated = false;

            try
            {
                string query = "UPDATE [Projects] SET [Status] = 'Completed' " +
                               "WHERE [ProjectID] = @ProjectID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectID", projectID);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }
            }
            catch(SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isUpdated;
        }
    }
}
