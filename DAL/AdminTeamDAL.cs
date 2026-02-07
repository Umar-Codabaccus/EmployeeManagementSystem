using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace EmployeeManagementSystem.DAL
{
    public class AdminTeamDAL
    {
        private Common common;

        public AdminTeamDAL()
        {
            common = new Common();
        }

        public List<Team> GetAllTeams()
        {
            List<Team> teams = new List<Team>();
            try
            {
                string query = "SELECT [Teams].[TeamID], [TeamName], [DepartmentName], " +
                               "[Departments].[DepartmentID], " +
                               "COUNT([Employees].[EmployeeID]) AS NumberOfEmployees " +
                               "FROM [Teams] LEFT JOIN [Departments] " +
                               "ON [Teams].[DepartmentID] = [Departments].[DepartmentID] " +
                               "LEFT JOIN [EmployeeTeams] " +
                               "ON [EmployeeTeams].[TeamID] = [Teams].[TeamID] " +
                               "LEFT JOIN [Employees] " +
                               "ON [EmployeeTeams].[EmployeeID] = [Employees].[EmployeeID] " +
                               "GROUP BY [Teams].[TeamID], [TeamName], [DepartmentName], [Departments].[DepartmentID] ";
                SqlCommand command = new SqlCommand();
                command.CommandText = query;
                command.Connection = common.GetConnection();

                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Team team = new Team();

                        team.TeamID = (int)reader["TeamID"];
                        team.TeamName = (string)reader["TeamName"];
                        team.DepartmentID = reader["DepartmentID"] == DBNull.Value ? 0 : (int)reader["DepartmentID"];
                        team.DepartmentName = reader["DepartmentName"] == DBNull.Value ? null : (string)reader["DepartmentName"];
                        team.NumberOfEmployees = reader["NumberOfEmployees"] == DBNull.Value ? 0 : (int)reader["NumberOfEmployees"];
                        
                        teams.Add(team);
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

            return teams;
        }

        public bool AddTeam(string teamName, int departmentID)
        {
            string tName = teamName;
            bool isDuplicate = CheckTeamName(tName);
            bool IsAdded = false;

            if (!isDuplicate)
            {
                try
                {
                    string query = $"INSERT INTO [Teams] ([TeamName], [DepartmentID])" +
                                   $"VALUES (@TeamName, @DepartmentID)";
                    SqlCommand command = new SqlCommand(query, common.GetConnection());
                    command.Parameters.AddWithValue("@TeamName", teamName);
                    command.Parameters.AddWithValue("@DepartmentID", departmentID);
                    common.OpenConnection();
                    command.ExecuteNonQuery();
                    IsAdded = true;
                }
                catch (SqlException ex)
                {
                    IsAdded = false;
                }
                finally { common.CloseConnection(); }
            }

            return IsAdded;
        }

        public bool CheckTeamName(string tName)
        {
            bool isDuplicate = false;

            try
            {
                string query = $"SELECT [TeamName] FROM [Teams] " +
                               $"WHERE [TeamName] = '{tName}'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isDuplicate = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { common.CloseConnection(); }

            return isDuplicate;
        }

        public bool DeleteTeam(int teamID)
        {
            string query = "DELETE FROM [Teams] WHERE [TeamID] = @TeamID";
            SqlCommand command = new SqlCommand(query, common.GetConnection());
            command.Parameters.AddWithValue("@TeamID", teamID);

            common.OpenConnection();
            int rowsAffected = command.ExecuteNonQuery();
            common.CloseConnection();
            return rowsAffected > 0;
        }
    }
}
