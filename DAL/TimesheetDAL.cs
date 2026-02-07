using EmployeeManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.DAL {
    public class TimesheetDAL {
        private Common common;
        private const int REGULAR_HOURS = 8;

        public TimesheetDAL() {
            common = new Common();
        }

        public bool CheckClockIn(int employeeID, DateTime workdate) {
            bool isClockedIn = false;

            try {
                string query = "SELECT TOP 1 [ClockOut] " +
                               "FROM [Attendances] " +
                               "WHERE [Workdate] = @Workdate " +
                               "AND [EmployeeID] = @EmployeeID " +
                               "ORDER BY [ClockOut] ASC";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@Workdate", workdate.ToString("yyyy/MM/dd"));
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) // If there is at least one row
                {
                    TimeSpan? clockOut = reader["ClockOut"] != DBNull.Value ? (TimeSpan)reader["ClockOut"] : (TimeSpan?)null;

                    if (!clockOut.HasValue) {
                        isClockedIn = true; // ClockOut is null, meaning the user has clocked in
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }

            return isClockedIn;
        }

        public void ClockIn(int employeeID, DateTime workdate, TimeSpan clockin) {

            try {
                string query = "INSERT INTO [Attendances] " +
                               "([EmployeeID], [Workdate], [ClockIn]) " +
                               "VALUES " +
                               "(@EmployeeID, @Workdate, @ClockIn)";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@Workdate", workdate.ToString("yyyy/MM/dd"));
                command.Parameters.AddWithValue("@ClockIn", clockin);

                common.OpenConnection();

                command.ExecuteNonQuery();
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }
        }

        public bool CheckClockOut(int employeeID, DateTime workdate) {
            bool checkClockOut = false;

            try {
                string query = "SELECT TOP 1 [ClockIn], [ClockOut] " +
                               "FROM [Attendances] " +
                               "WHERE [EmployeeID] = @EmployeeID " +
                               "AND [Workdate] = @Workdate " +
                               "ORDER BY [ClockIn] DESC";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@Workdate", workdate.ToString("yyyy/MM/dd"));

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        TimeSpan? clockIn = reader["ClockIn"] != DBNull.Value ? (TimeSpan)reader["ClockIn"] : (TimeSpan?)null;
                        TimeSpan? clockOut = reader["ClockOut"] != DBNull.Value ? (TimeSpan)reader["ClockOut"] : (TimeSpan?)null;

                        if (clockIn.HasValue && clockOut.HasValue) {
                            checkClockOut = true;
                        }
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }

            return checkClockOut;
        }

        public TimeSpan GetClockIn(int employeeID, DateTime workdate) {
            TimeSpan clockIn = DateTime.Now.TimeOfDay;

            try {
                string query = "SELECT TOP 1 [ClockIn] FROM [Attendances] " +
                               "WHERE [Workdate] = @Workdate " +
                               "AND [EmployeeID] = @EmployeeID " +
                               "ORDER BY [ClockIn] DESC";
                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@Workdate", workdate.ToString("yyyy/MM/dd"));
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        clockIn = (TimeSpan)reader["ClockIn"];
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }


            return clockIn;
        }

        public void ClockOut(int employeeID, DateTime workdate, TimeSpan clockOut) {
            TimeSpan clockIn = GetClockIn(employeeID, workdate);
            TimeSpan workDuration = clockOut - clockIn;
            double hoursWorked = workDuration.TotalHours;
            double overtime = 0.00;

            if (hoursWorked > REGULAR_HOURS) {
                overtime = hoursWorked - REGULAR_HOURS;
            }

            if (clockIn != DateTime.Now.TimeOfDay) {
                try {
                    string query = "UPDATE [Attendances] " +
                                   "SET [ClockOut] = @ClockOut, " +
                                   "[HoursWorked] = @HoursWorked, " +
                                   "[Overtime] = @Overtime " +
                                   "WHERE [Workdate] = @Workdate " +
                                   "AND [EmployeeID] = @EmployeeID " +
                                   "AND [ClockIn] = @ClockIn";

                    SqlCommand command = new SqlCommand(query, common.GetConnection());
                    command.Parameters.AddWithValue("@Workdate", workdate.ToString("yyyy/MM/dd"));
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    command.Parameters.AddWithValue("@ClockOut", clockOut);
                    command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                    command.Parameters.AddWithValue("@Overtime", overtime);
                    command.Parameters.AddWithValue("@ClockIn", clockIn);

                    common.OpenConnection();

                    command.ExecuteNonQuery();
                } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }
            }
        }

        public List<Timesheet> GetAttendances(int employeeID) {
            List<Timesheet> attendances = new List<Timesheet>();

            try {
                string query = "SELECT [Workdate], [ClockIn], [ClockOut], [HoursWorked], [Overtime]" +
                               "FROM [Attendances] " +
                               "WHERE [EmployeeID] = @EmployeeID " +
                               "ORDER BY [Workdate], [ClockIn] DESC";

                SqlCommand command = new SqlCommand(query, common.GetConnection());
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                common.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Timesheet attendance = new Timesheet();
                        attendance.Workdate = reader["Workdate"] != DBNull.Value ? (DateTime)reader["Workdate"] : default;
                        attendance.ClockIn = reader["ClockIn"] != DBNull.Value ? (TimeSpan)reader["ClockIn"] : default;
                        attendance.ClockOut = reader["ClockOut"] != DBNull.Value ? (TimeSpan)reader["ClockOut"] : default;
                        attendance.HoursWorked = reader["HoursWorked"] != DBNull.Value ? Convert.ToSingle(reader["HoursWorked"]) : 0;
                        attendance.Overtime = reader["Overtime"] != DBNull.Value ? Convert.ToSingle(reader["Overtime"]) : 0;

                        attendances.Add(attendance);
                    }
                }
            } catch (SqlException ex) { throw new Exception(ex.Message); } finally { common.CloseConnection(); }

            return attendances;
        }
    }
}
