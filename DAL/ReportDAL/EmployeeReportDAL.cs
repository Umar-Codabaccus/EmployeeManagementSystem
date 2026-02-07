using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagementModels.ReportManagement;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL.ReportDAL
{
    public class EmployeeReportDAL
    {
        private Common common;

        public EmployeeReportDAL()
        {
            common = new Common();
        }

        public string GetPositionName(int positionID)
        {
            string positionName = "";

            try
            {
                string query = "SELECT [PositionName] FROM [Positions] " +
                               "WHERE [PositionID] = @PositionID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@PositionID", positionID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        positionName = (string)reader["PositionName"];
                    }
                }
            }
            catch (SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return positionName;
        }  
        public string GetDepartmentName(int departmentID)
        {
            string departmentName = "";

            try
            {
                string query = "SELECT [DepartmentName] FROM [Departments] " +
                               "WHERE [DepartmentID] = @DepartmentID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

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
        public List<Position> GetPositions(string keyword)
        {
            List<Position> positions = new List<Position>();

            try
            {
                string query = $"SELECT [PositionID], [PositionName] FROM [Positions] " +
                               $"WHERE [PositionName] LIKE '%{keyword}%'";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Position position = new Position();
                        position.PositionID = (int)reader["PositionID"];
                        position.PositionName = (string)reader["PositionName"];

                        positions.Add(position);
                    }
                }
            }
            catch(SqlException ex) { throw new Exception(ex.Message); }
            finally { common.CloseConnection(); }

            return positions;
        }
        public List<Department> GetDepartments(string keyword)
        {
            List<Department> departments = new List<Department>();

            try
            {
                string query = $"SELECT [DepartmentID], [DepartmentName] FROM [Departments] " +
                               $"WHERE [DepartmentName] LIKE '%{keyword}%'";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Department department = new Department();
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
        public List<EmployeeModel> GetEmployees(int ID, string sort)
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();

            switch (sort)
            {
                case "position-none":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [DepartmentName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Positions].[PositionID] =@PositionID";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@PositionID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.DepartmentName = (string)reader["DepartmentName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "position-name-asc":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [DepartmentName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Positions].[PositionID] =@PositionID " +
                                       "ORDER BY [Firstname] ASC, [Lastname] ASC";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@PositionID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.DepartmentName = (string)reader["DepartmentName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "position-name-desc":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [DepartmentName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Positions].[PositionID] =@PositionID " +
                                       "ORDER BY [Firstname] DESC, [Lastname] DESC";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@PositionID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.DepartmentName = (string)reader["DepartmentName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "position-salary":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [DepartmentName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Positions].[PositionID] =@PositionID " +
                                       "ORDER BY [Salary] DESC";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@PositionID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.DepartmentName = (string)reader["DepartmentName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "department-none":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [PositionName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Departments].[DepartmentID] = @DepartmentID";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@DepartmentID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.PositionName = (string)reader["PositionName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "department-name-asc":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [PositionName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Departments].[DepartmentID] = @DepartmentID " +
                                       "ORDER BY [Firstname] ASC, [Lastname]  ASC";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@DepartmentID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.PositionName = (string)reader["PositionName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "department-name-desc":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [PositionName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Departments].[DepartmentID] = @DepartmentID " +
                                       "ORDER BY [Firstname] DESC, [Lastname] DESC";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@DepartmentID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.PositionName = (string)reader["PositionName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;

                case "department-salary":
                    try
                    {
                        string query = "SELECT [Firstname], [Lastname], [TeamName], [PositionName], " +
                                       "[Salary] " +
                                       "FROM [Employees] INNER JOIN [Positions] " +
                                       "ON [Employees].[PositionID] = [Positions].[PositionID] " +
                                       "INNER JOIN [EmployeeTeams] " +
                                       "ON [Employees].[EmployeeID] = [EmployeeTeams].[EmployeeID] " +
                                       "INNER JOIN [Teams] " +
                                       "ON [Teams].[TeamID] = [EmployeeTeams].[TeamID] " +
                                       "INNER JOIN [Departments] " +
                                       "ON [Departments].[DepartmentID] = [Teams].DepartmentID " +
                                       "WHERE [Departments].[DepartmentID] = @DepartmentID " +
                                       "ORDER BY [Salary] DESC";

                        SqlCommand command = new SqlCommand(query, common.GetConnection());
                        command.Parameters.AddWithValue("@DepartmentID", ID);

                        common.OpenConnection();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel();
                                employee.EmployeeName = $"{(string)reader["Firstname"]} {(string)reader["Lastname"]}";
                                employee.TeamName = (string)reader["TeamName"];
                                employee.PositionName = (string)reader["PositionName"];
                                employee.Salary = (int)reader["Salary"];

                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException ex) { throw new Exception(ex.Message); }
                    finally { common.CloseConnection(); }
                    break;
            }

            return employees;
        }
    }
}
