using EmployeeManagementSystem.ViewModels.EmployeeViewModels;
using EmployeeManagementSystem.DAL.EmployeeDAL;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagementSystem.DAL;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private EmployeeDashboardDAL dashboard;
        private ProjectDAL project;
        private TimesheetDAL timesheet;

        public EmployeeController()
        {
            dashboard = new EmployeeDashboardDAL();
            project = new ProjectDAL();
            timesheet = new TimesheetDAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            int employeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            EmployeeDashViewModel employeeDashVM = new EmployeeDashViewModel();
            employeeDashVM.EmployeeID = employeeID;

            employeeDashVM.EmployeeName = dashboard.GetEmployeeName(employeeDashVM.EmployeeID);
            employeeDashVM.TeamID = dashboard.GetTeamID(employeeDashVM.EmployeeID);
            employeeDashVM.TeamName = dashboard.GetTeamName(employeeDashVM.TeamID);
            employeeDashVM.DepartmentName = dashboard.GetDepartmentName(employeeDashVM.TeamID);
            employeeDashVM.ProjectList = dashboard.GetProjectSpecifications(employeeDashVM.EmployeeID);
            return View(employeeDashVM);
        }

        [HttpPost]
        public IActionResult Index(EmployeeDashViewModel employeeDashVM, string navOption)
        {
            int employeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            employeeDashVM.EmployeeID = employeeID;
            employeeDashVM.TeamID = dashboard.GetTeamID(employeeDashVM.EmployeeID);
            switch (navOption)
            {
                case "project":
                    return RedirectToAction("ProjectSpecification", "Employee",
                        new
                        {
                            employeeID = employeeDashVM.EmployeeID,
                            teamID = employeeDashVM.TeamID
                        });
                case "team":
                    return RedirectToAction("MyTeam", "Employee",
                        new
                        {
                            employeeID = employeeDashVM.EmployeeID,
                            teamID = employeeDashVM.TeamID
                        });
                case "timesheet":
                    return RedirectToAction("TimesheetEmployeeDashboard", "Employee",
                        new
                        {
                            employeeID = employeeDashVM.EmployeeID,
                            teamID = employeeDashVM.TeamID
                        });
                case "profile:":
                    return RedirectToAction("EmployeeProfile", "Employee",
                        new
                        {
                            employeeID = employeeDashVM.EmployeeID,
                            teamID = employeeDashVM.TeamID
                        });
                case "logout":
                    return RedirectToAction("login", "Login");
            }

            employeeDashVM.EmployeeName = dashboard.GetEmployeeName(employeeDashVM.EmployeeID);
            employeeDashVM.TeamName = dashboard.GetTeamName(employeeDashVM.TeamID);
            employeeDashVM.DepartmentName = dashboard.GetDepartmentName(employeeDashVM.TeamID);
            employeeDashVM.ProjectList = dashboard.GetProjectSpecifications(employeeDashVM.EmployeeID);
            return View(employeeDashVM);
        }

        [HttpGet]
        public IActionResult ProjectSpecification()
        {
            int employeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            ViewBag.NotUpdated = null;
            EmployeeProjectVM employeeProjectVM = new EmployeeProjectVM();
            employeeProjectVM.EmployeeID = employeeID;
            employeeProjectVM.TeamID = dashboard.GetTeamID(employeeProjectVM.EmployeeID);
            employeeProjectVM.Projects = project.GetProjectDashboardInfo(employeeProjectVM.EmployeeID);
            return View(employeeProjectVM);
        }

        [HttpPost]
        public IActionResult ProjectSpecification(EmployeeProjectVM employeeProjectVM, string navOption)
        {
            int employeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            ViewBag.NotUpdated = null;

            employeeProjectVM.TeamID = dashboard.GetTeamID(employeeProjectVM.EmployeeID);
            switch (navOption)
            {
                case "dashboard":
                    return RedirectToAction("Index", "Employee",
                    new
                    {
                        employeeID = employeeProjectVM.EmployeeID,
                        teamID = employeeProjectVM.TeamID
                    });
                case "team":
                    return RedirectToAction("MyTeam", "Employee",
                        new
                        {
                            employeeID = employeeProjectVM.EmployeeID,
                            teamID = employeeProjectVM.TeamID
                        });
                case "timesheet":
                    return RedirectToAction("TimesheetEmployeeDashboard", "Employee",
                        new
                        {
                            employeeID = employeeProjectVM.EmployeeID,
                            teamID = employeeProjectVM
                        });
                case "profile:":
                    return RedirectToAction("EmployeeProfile", "Employee",
                        new
                        {
                            employeeID = employeeProjectVM.EmployeeID,
                            teamID = employeeProjectVM.TeamID
                        });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            if (employeeProjectVM.SpecificationID == 0)
            {
                employeeProjectVM.Projects = project.GetProjectDashboardInfo(employeeProjectVM.EmployeeID);
                return View(employeeProjectVM);
            }

            bool isUpdated = false;
            isUpdated = project.UpdateSpecificationStatus(employeeProjectVM.EmployeeID, employeeProjectVM.SpecificationID);

            if (!isUpdated)
            {
                ViewBag.NotUpdated = "Something went wrong.";
            }

            employeeProjectVM.Projects = project.GetProjectDashboardInfo(employeeProjectVM.EmployeeID);
            return View(employeeProjectVM);
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
        public IActionResult TimesheetEmployeeDashboard(int teamID)
        {
            int employeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
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
        public IActionResult TimesheetEmployeeDashboard(ClockInOutVM clock, string navOption, string clockButton)
        {
            // Retrieve the logged-in manager's ID from claims
            int employeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            clock.EmployeeID = employeeID;
            ViewBag.Error = null;

            switch (navOption)
            {
                case "dashboard":
                    return RedirectToAction("Index", "Employee",
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
                case "team":
                    return RedirectToAction("MyTeam", "Employee",
                    new
                    {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "profile:":
                    return RedirectToAction("Profile", "Employee",
                    new
                    {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "logout":
                    return RedirectToAction("Logout", "Login");
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
