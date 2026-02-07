using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagementSystem.DAL {
    public class AdminDepartmentDAL {
        private string query;
        private Common common;
        private SqlCommand sqlCommand;
        private SqlDataReader reader;

        public AdminDepartmentDAL() {
            query = "";
            common = new Common();
            reader = null;
        }

        public List<Department> GetDepartments() {
            List<Department> departments = new List<Department>();

            try {
                query = "SELECT [Departments].[DepartmentID], [DepartmentName], " +
                        "COUNT(DISTINCT [Teams].[TeamID]) AS NumberOfTeams, " +
                        "COUNT(DISTINCT [Employees].[EmployeeID]) As NumberOfEmployees " +
                        "FROM [Departments] LEFT JOIN [Teams] " +
                        "ON [Departments].[DepartmentID] = [Teams].[DepartmentID] " +
                        "LEFT JOIN [EmployeeTeams] " +
                        "ON [EmployeeTeams].[TeamID] = [Teams].[TeamID] " +
                        "LEFT JOIN [Employees] " +
                        "ON [EmployeeTeams].[EmployeeID] = [Employees].[EmployeeID] " +
                        "GROUP BY [Departments].[DepartmentID], [DepartmentName];";
                sqlCommand = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                reader = sqlCommand.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Department department = new Department();
                        department.DepartmentID = (int)reader["DepartmentID"];
                        department.DepartmentName = (string)reader["DepartmentName"];
                        department.NumberOfTeams = reader["NumberOfTeams"] == DBNull.Value ? 0 : (int)reader["NumberOfTeams"];
                        department.NumberOfEmployees = reader["NumberOfEmployees"] == DBNull.Value ? 0 : (int)reader["NumberOfEmployees"];

                        departments.Add(department);
                    }
                }
            } catch (SqlException ex) {
                Console.WriteLine("Error: " + ex.Message);
            } finally {
                common.CloseConnection();
            }

            return departments;
        }


        public bool AddNewDepartment(string departmentName) {
            string dpName = departmentName;
            bool isDuplicate = CheckDepartmentName(dpName);
            bool addSuccessful = false;

            if (!isDuplicate) {
                try {
                    query = "spAddDepartment";
                    sqlCommand = new SqlCommand(query, common.GetConnection());
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", departmentName);

                    common.OpenConnection();

                    sqlCommand.ExecuteNonQuery();

                    addSuccessful = true;
                } catch (SqlException ex) {
                    addSuccessful = false;
                } finally { common.CloseConnection(); }
            }

            return addSuccessful;
        }

        public bool CheckDepartmentName(string dpName) {
            bool isDuplicate = false;

            try {
                string query = $"SELECT [DepartmentName] FROM [Departments] " +
                               $"WHERE [DepartmentName] = '{dpName}'";
                sqlCommand = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();
                reader = sqlCommand.ExecuteReader();

                if (reader.HasRows) {
                    isDuplicate = true;
                }
            } catch (SqlException ex) {
                Console.WriteLine(ex.Message);
            } finally { common.CloseConnection(); }

            return isDuplicate;
        }

        public bool DeleteDepartment(int departmentID) {
            string query = $"DELETE FROM [Departments] WHERE [DepartmentID] = {departmentID}";

            sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Connection = common.GetConnection();
            common.OpenConnection();
            int rowsAffected = sqlCommand.ExecuteNonQuery();
            common.CloseConnection();
            return (rowsAffected > 0);
        }

    }
}
