using Microsoft.Data.SqlClient;
using EmployeeManagementSystem.Models.EmployeeModels.DashboardModels;


namespace EmployeeManagementSystem.DAL.EmployeeDAL
{
    public class ProjectDAL
    {
        private Common common;

        public ProjectDAL()
        {
            common = new Common();
        }


        public List<ProjectDashboard> GetProjectDashboardInfo(int employeeID)
        {
            List<ProjectDashboard> projects = new List<ProjectDashboard>();

            try
            {
                string query = "SELECT [Specifications].[SpecificationID], [Description], " +
                               "[ProjectName] " +
                               "FROM [Specifications] INNER JOIN [Projects] " +
                               "ON [Specifications].[ProjectID] = [Projects].[ProjectID] " +
                               "INNER JOIN [Employees] " +
                               "ON [Specifications].[EmployeeID] = [Employees].[EmployeeID] " +
                               "WHERE [Specifications].[Status] = 'In Progress' " +
                               "AND [Employees].[EmployeeID] = @EmployeeID";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProjectDashboard project = new ProjectDashboard();
                        project.SpecificationID = (int)reader["SpecificationID"];
                        project.Description = (string)reader["Description"];
                        project.ProjectName = (string)reader["ProjectName"];

                        projects.Add(project);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return projects;
        }

        public bool UpdateSpecificationStatus(int employeeID, int specificationID)
        {
            bool isUpdated = false;

            try
            {
                string query = "UPDATE [Specifications] SET [Status] = 'Completed' " +
                               "WHERE [EmployeeID] = @EmployeeID " +
                               "AND [SpecificationID] = @SpecificationID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@SpecificationID", specificationID);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isUpdated;
        }
    }
}
