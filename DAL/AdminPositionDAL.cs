using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL
{
    public class AdminPositionDAL
    {
        private Common common;

        public AdminPositionDAL()
        {
            common = new Common();
        }

        public List<Position> GetAllPositions()
        {
            List<Position> positions = new List<Position>();

            string query = "SELECT [PositionID], [PositionName] FROM [Positions]";
            SqlCommand command = new SqlCommand();
            command.CommandText = query;
            command.Connection = common.GetConnection();

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

            return positions;
        }

        public bool AddPosition(string positionName)
        {
            string pName = positionName;
            bool IsDuplicate = CheckPositionName(pName);
            bool IsAdded = false;

            if (!IsDuplicate)
            {
                try
                {
                    string query = $"INSERT INTO [Positions] ([PositionName]) VALUES ('{positionName}')";
                    SqlCommand command = new SqlCommand();
                    command.CommandText = query;
                    command.Connection = common.GetConnection();

                    common.OpenConnection();

                    command.ExecuteNonQuery();

                    IsAdded = true;
                }
                catch (SqlException ex)
                {
                    IsAdded = false;
                }
                finally
                {
                    common.CloseConnection();
                }
            }
            
            
            return IsAdded;
        }

        public bool CheckPositionName(string pName)
        {
            bool duplicate = false;
            try
            {
                string query = $"SELECT [PositionName] FROM [Positions] WHERE [PositionName] = '{pName}'";
                SqlCommand command = new SqlCommand();
                command.CommandText = query;
                command.Connection = common.GetConnection();
                common.OpenConnection();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    duplicate = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                common.CloseConnection();
            }

            return duplicate;
        }

        public bool DeletePosition(int positionID)
        {
            bool isDeleted = false;
            try
            {
                string query = $"DELETE FROM [Positions] WHERE [PositionID] = '{positionID}'";
                SqlCommand command = new SqlCommand();
                command.CommandText = query;
                command.Connection = common.GetConnection();

                common.OpenConnection();
                command.ExecuteNonQuery();

                isDeleted = true;
            }
            catch (SqlException ex)
            {
                isDeleted = false;
            }
            finally { common.CloseConnection(); }

            return isDeleted;
        }
    }
}
