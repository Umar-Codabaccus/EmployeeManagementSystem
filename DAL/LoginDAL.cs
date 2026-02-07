using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagementSystem.DAL {
    public class LoginDAL {
        private Common common;
        public LoginDAL() {
            common = new Common();
        }

        public Login RetrieveLoginDetails(string username, string password) {
            Login login = new Login();
            login.LoginSuccessfull = false;

            try {
                string query = "SELECT [Employees].[EmployeeID], [Roles].[RoleID], [RoleName] " +
                        "FROM [Users] INNER JOIN [Employees] ON [Users].[EmployeeID] = [Employees].[EmployeeID] " +
                        "INNER JOIN [Roles] ON [Users].[RoleID] = [Roles].[RoleID] " +
                        "WHERE [Username] COLLATE Latin1_General_CS_AS = @Username " +
                        "AND [Password]  COLLATE Latin1_General_CS_AS = @Password";
                SqlCommand sqlCommand = new SqlCommand(query, common.GetConnection());
                sqlCommand.Parameters.AddWithValue("@Username", username);
                sqlCommand.Parameters.AddWithValue("@Password", password);

                common.OpenConnection();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                int temp;
                string errorMessage = null;
                if (reader.HasRows) {
                    while (reader.Read()) {
                        login.RoleID = (int)reader["RoleID"];
                        login.Username = username;
                        login.EmployeeID = (int)reader["EmployeeID"];
                        login.RoleName = (string)reader["RoleName"];
                        login.LoginSuccessfull = true;
                        login.UsernameEntered = true;
                    }
                }

                if (!login.LoginSuccessfull) {
                    login.UsernameEntered = true;
                }
            } catch (Exception ex) {
                login.UsernameEntered = false;
            } finally {
                common.CloseConnection();
            }

            return login;
        }
    }
}