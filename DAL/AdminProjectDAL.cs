using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagementModels.ProjectManagement;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL
{
    public class AdminProjectDAL
    {
        private Common common;

        public AdminProjectDAL()
        {
            common = new Common();
        }

        public int GetDepartmentByKeyword(string keyword)
        {
            int departmentID = 0;

            try
            {
                string query = $"SELECT [DepartmentID] FROM [Departments] " +
                               $"WHERE [DepartmentName] LIKE '%{keyword}%'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        departmentID = (int)reader["DepartmentID"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return departmentID;
        }
        public List<TeamProj> GetTeamsByKeyword(int departmentID)
        {
            List<TeamProj> teams = new List<TeamProj>();

            try
            {
                string query = "SELECT [Teams].[TeamID], [TeamName], [DepartmentName], " +
                               "COUNT([Projects].[ProjectID]) AS NumProjects " +
                               "FROM [Teams] INNER JOIN [Departments] " +
                               "ON [Teams].[DepartmentID] = [Departments].[DepartmentID] " +
                               "LEFT JOIN [Projects] " +
                               "ON [Teams].[TeamID] = [Projects].[TeamID] " +
                               "WHERE [Departments].[DepartmentID] = @DepartmentID " +
                               "GROUP BY [Teams].[TeamID], [TeamName], [DepartmentName]";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@DepartmentID",  departmentID);
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TeamProj team = new TeamProj();
                        team.TeamID = (int)reader["TeamID"];
                        team.TeamName = (string)reader["TeamName"];
                        team.DepartmentName = (string)reader["DepartmentName"];
                        team.NumProjects = reader["NumProjects"] == DBNull.Value ? 0 : (int)reader["NumProjects"];

                        teams.Add(team);
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return teams;
        }
        public bool CreateNewProject(string projectName, int teamID, DateTime deadline)
        {
            bool isProjectCreated = false;
            string status = "In Progress";

            try
            {
                string query = "INSERT INTO [Projects] ([ProjectName], [TeamID], [Status], [Deadline]) " +
                               "VALUES (@ProjectName, @TeamID, @Status, @Deadline)";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@ProjectName", projectName);
                command.Parameters.AddWithValue("@TeamID", teamID);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@Deadline", deadline.ToString("yyyy/MM/dd"));

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isProjectCreated = true;
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return isProjectCreated;
        }
    }
}
