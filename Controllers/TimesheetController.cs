using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.DAL.ManagerDAL;
using EmployeeManagementSystem.ViewModels;
using EmployeeManagementSystem.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeManagementSystem.Controllers
{
    public class TimesheetController : Controller
    {
        private TimesheetDAL timesheet;
        private ManagerDashboardDAL fetchTeamID;

        public TimesheetController()
        {
            timesheet = new TimesheetDAL();
            fetchTeamID = new ManagerDashboardDAL();
        }
        public static string GetFullDate(DateTime date)
        {
            int day = date.Day;
            string suffix = day % 10 == 1 && day != 11 ? "st" :
                            day % 10 == 2 && day != 12 ? "nd" :
                            day % 10 == 3 && day != 13 ? "rd" : "th";

            return $"{date:MMMM} {day}{suffix}, {date:yyyy}";
        }
        [HttpGet]
        public IActionResult TimesheetDashboard(int employeeID, int teamID)
        {
            
            ViewBag.Error = null;

            ClockInOutVM clock = new ClockInOutVM();
            clock.EmployeeID = employeeID;
            clock.TeamID = teamID;
            clock.Workdate = (DateTime.Today);
            clock.DisplayWorkdate = GetFullDate(clock.Workdate);
            clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
            clock.Workdate = DateTime.Now;
            return View(clock);
        }

        [HttpPost]
        public IActionResult TimesheetDashboard(ClockInOutVM clock, string navOption, string clockButton)
        {
            ViewBag.Error = null;

            switch (navOption)
            {
                case "dashboard":
                    return RedirectToAction("EmployeeDashboard", "Employee",
                    new
                    {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "team":
                    return RedirectToAction("MyTeam", "Employee",
                    new
                    {
                        employeeID = clock.EmployeeID,
                            teamID = clock.TeamID
                        });
                case "project":
                    return RedirectToAction("ProjectSpecification", "Employee",
                    new
                    {
                            employeeID = clock.EmployeeID,
                            teamID = clock.TeamID
                        });
                case "profile:":
                    return RedirectToAction("EmployeeProfile", "Employee",
                    new
                    {
                            employeeID = clock.EmployeeID,
                            teamID = clock.TeamID
                        });
                case "logout":
                    return RedirectToAction("login", "Login");
            }
         
            switch (clockButton)
            {
                case "clockin":
                    clock.Workdate = DateTime.Now;
                    bool checkClockedIn = timesheet.CheckClockIn(clock.EmployeeID, clock.Workdate);
                    if (checkClockedIn)
                    {
                        ViewBag.Error = "You are already clocked in.";
                        clock.Workdate = (DateTime.Today);
                        clock.DisplayWorkdate = GetFullDate(clock.Workdate);
                        clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
                        return View(clock);
                    }
                    clock.Workdate = DateTime.Now;
                    clock.ClockIn = clock.Workdate.TimeOfDay;
                    timesheet.ClockIn(clock.EmployeeID, clock.Workdate, clock.ClockIn);
                    clock.Workdate = (DateTime.Today);
                    clock.DisplayWorkdate = GetFullDate(clock.Workdate);
                    clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
                    return View(clock);
                case "clockout":
                    clock.Workdate = DateTime.Now;
                    bool checkClockedOut = timesheet.CheckClockOut(clock.EmployeeID, clock.Workdate);
                    if (checkClockedOut)
                    {
                        ViewBag.Error = "You can't clock out. You need to clock in.";
                        clock.Workdate = (DateTime.Today);
                        clock.DisplayWorkdate = GetFullDate(clock.Workdate);
                        clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
                        return View(clock);
                    }
                    clock.Workdate = DateTime.Now;
                    clock.ClockOut = clock.Workdate.TimeOfDay;
                    timesheet.ClockOut(clock.EmployeeID, clock.Workdate, clock.ClockOut);
                    clock.Workdate = (DateTime.Today);
                    clock.DisplayWorkdate = GetFullDate(clock.Workdate);
                    clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
                    return View(clock);
            }

            clock.Workdate = (DateTime.Today);
            clock.DisplayWorkdate = GetFullDate(clock.Workdate);
            clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
            return View(clock);
        }
    }
}
