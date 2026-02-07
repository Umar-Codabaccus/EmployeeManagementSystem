using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagementSystem.DAL
{
    public class Common
    {
        private string connectionString;
        private SqlConnection connection;

        public Common()
        {
            connectionString = "Data Source=DAYFORCEPJSEXUL\\SQLEXPRESS;Initial Catalog=employeeManagementSystemDB;Integrated Security=True;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public void OpenConnection()
        {
            connection.Open();

            if (connection.State != ConnectionState.Open) { throw new System.NotSupportedException(); }
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
