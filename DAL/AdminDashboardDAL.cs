using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL {
    public class AdminDashboardDAL {
        private Common common;

        public AdminDashboardDAL() {
            common = new Common();
        }

        public List<PolarChart> GetPolarChartInfo() {
            List<PolarChart> polarChart = new List<PolarChart>();

            try {
                string query = "SELECT [TeamName], COUNT([Employees].[EmployeeID]) AS NumEmployees " +
                               "FROM [Teams] LEFT JOIN [EmployeeTeams] " +
                               "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                               "LEFT JOIN [Employees] " +
                               "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                               "GROUP BY [TeamName]";

                SqlCommand command = new SqlCommand(query, common.GetConnection());

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) {
                    while (reader.Read()) {
                        PolarChart chart = new PolarChart();
                        chart.TeamName = (string)reader["TeamName"];
                        chart.NumEmployees = reader["NumEmployees"] == DBNull.Value ? 0 : (int)reader["NumEmployees"];

                        polarChart.Add(chart);
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }

            return polarChart;
        }

        public List<BarChart> GetBarChartInfo() {
            List<BarChart> barChart = new List<BarChart>();

            try {
                string query = "SELECT [DepartmentName], COUNT(DISTINCT [Teams].[TeamID]) AS NumTeams " +
                               "FROM [Teams] LEFT JOIN [Departments] " +
                               "ON [Departments].[DepartmentID] = [Teams].[DepartmentID] " +
                               "GROUP BY [DepartmentName]";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) {
                    while (reader.Read()) {
                        BarChart chart = new BarChart();
                        chart.DepartmentName = reader["DepartmentName"] == DBNull.Value ? null : (string)reader["DepartmentName"];
                        chart.NumTeams = reader["NumTeams"] == DBNull.Value ? 0 : (int)reader["NumTeams"];

                        barChart.Add(chart);
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }

            return barChart;
        }

        public List<ProjectList> GetProjectList() {
            List<ProjectList> projects = new List<ProjectList>();

            try {
                string query = "SELECT [ProjectName], [TeamName], [Deadline], [Status] " +
                               "FROM [Projects] INNER JOIN [Teams] " +
                               "ON [Projects].[TeamID] = [Teams].[TeamID];";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) {
                    while (reader.Read()) {
                        ProjectList projectList = new ProjectList();
                        projectList.ProjectName = (string)reader["ProjectName"];
                        projectList.TeamName = (string)reader["TeamName"];
                        projectList.Deadline = (DateTime)reader["Deadline"];
                        projectList.Status = (string)reader["Status"];

                        projects.Add(projectList);
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }

            return projects;
        }
    }
}
