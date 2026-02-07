using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL
{
    public class AdminUpdateDAL
    {
        private Common common;

        public AdminUpdateDAL()
        {
            common = new Common();
        }

        public bool UpdateEmployeeName(int  employeeID, string firstName, string lastName)
        {
            bool isUpdated = false;

            try
            {
                string query = "UPDATE [Employees] " +
                               "SET [Firstname] = @FirstName, [Lastname] = @LastName " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return isUpdated;
        }

        public bool UpdateEmployeeEmail(int employeeID, string email)
        {
            bool isUpdated = false;

            try
            {
                string query = "UPDATE [Employees] SET [Email] = @Email " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@Email", email);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return isUpdated;
        }

        public bool UpdateEmployeeAddress(int employeeID, string city, string street)
        {
            bool isAddressUpdated = false;

            try
            {
                string query = "UPDATE [Employees] SET [City] = @City, [Street] = @Street " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@Street", street);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isAddressUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return isAddressUpdated;
        }

        public bool UpdateEmployeePhoneNumber(int employeeID, int phoneNumber)
        {
            bool isPhoneUpdated = false;

            try
            {
                string query = "UPDATE [Employees] SET [Phone] = @Phone " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@Phone", phoneNumber);

                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isPhoneUpdated = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
            finally { common.CloseConnection(); }

            return isPhoneUpdated;
        }

        public bool UpdateEmployeeSalary(int employeeID, int salary)
        {
            bool isSalaryUpdated = false;

            try
            {
                string query = "UPDATE [Employees] SET [Salary] = @Salary " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@Salary", salary);

                common.OpenConnection();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isSalaryUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return isSalaryUpdated;
        }

        public List<EmployeeDetails> GetPositionByKeywordForUpdate(string keyword)
        {
            List<EmployeeDetails> positions = new List<EmployeeDetails>();

            try
            {
                string query = $"SELECT [PositionID], [PositionName] " +
                               $"FROM [Positions] " +
                               $"WHERE [PositionName] LIKE '%{keyword}%' " +
                               $"AND [PositionName] NOT LIKE 'CEO'";
                SqlCommand command = new SqlCommand(query, common.GetConnection());

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmployeeDetails emp = new EmployeeDetails();
                        emp.PositionID = (int)reader["PositionID"];
                        emp.PositionName = (string)reader["PositionName"];
                        positions.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return positions;
        }

        public bool UpdateEmployeePosition(int employeeID, int positionID)
        {
            bool isPositionUpdated = false;

            try
            {
                string query = "UPDATE [Employees] SET [PositionID] = @PositionID " +
                               "WHERE [EmployeeID] = @EmployeeID";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@PositionID", positionID);
                common.OpenConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isPositionUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally { common.CloseConnection(); }

            return isPositionUpdated;
        }
    }
}
