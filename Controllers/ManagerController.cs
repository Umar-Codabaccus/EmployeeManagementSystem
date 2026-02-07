using EmployeeManagementSystem.ViewModels.ManagerViewModels;
using EmployeeManagementSystem.DAL.ManagerDAL;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.ViewModels.ProjectManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagementSystem.ViewModels;
using EmployeeManagementSystem.DAL;

namespace EmployeeManagementSystem.Controllers {
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller {
        private ManagerDashboardDAL managerDash;
        private ProjectManagerDAL projectManager;
        private TimesheetDAL timesheet;

        public ManagerController() {
            managerDash = new ManagerDashboardDAL();
            projectManager = new ProjectManagerDAL();
            timesheet = new TimesheetDAL();
        }

        [HttpGet]
        public IActionResult Index() {
            // Retrieve the logged-in admin's ID from claims
            int managerID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");

            DashboardViewModel dashboardVM = new DashboardViewModel();
            dashboardVM.ManagerID = managerID;

            dashboardVM.ManagerName = managerDash.FindManagerName(dashboardVM.ManagerID);
            dashboardVM.TeamID = managerDash.FindTeamID(dashboardVM.ManagerID);
            dashboardVM.TeamName = managerDash.FindTeamName(dashboardVM.TeamID);
            dashboardVM.DepartmentName = managerDash.FindDepartmentName(dashboardVM.TeamID);
            dashboardVM.ProjectList = managerDash.GetProjectDetails(dashboardVM.TeamID);

            return View(dashboardVM);
        }

        [HttpPost]
        public IActionResult Index(DashboardViewModel dashboardVM, string navOption) {
            // Retrieve the logged-in manager's ID from claims
            int managerID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            dashboardVM.ManagerID = managerID;
            dashboardVM.TeamID = managerDash.FindTeamID(dashboardVM.ManagerID);

            switch (navOption) {
                case "project":
                    dashboardVM.TeamID = managerDash.FindTeamID(dashboardVM.ManagerID);
                    return RedirectToAction("ProjectManagement", "Manager", new {
                        managerID = dashboardVM.ManagerID,
                        teamID = dashboardVM.TeamID
                    });
                case "employee":
                    return RedirectToAction("EmployeeManagement", "Manager", new {
                        managerID = dashboardVM.ManagerID,
                        teamID = dashboardVM.TeamID
                    });
                case "timesheet":
                    return RedirectToAction("TimesheetDashboard", "Manager", new {
                        managerID = dashboardVM.ManagerID,
                        teamID = dashboardVM.TeamID
                    });
                case "profile":
                    return RedirectToAction("Profile", "Manager",
                        new {
                            managerID = dashboardVM.ManagerID,
                            teamID = dashboardVM.TeamID
                        });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            dashboardVM.ManagerName = managerDash.FindManagerName(dashboardVM.ManagerID);
            dashboardVM.TeamID = managerDash.FindTeamID(dashboardVM.ManagerID);
            dashboardVM.TeamName = managerDash.FindTeamName(dashboardVM.TeamID);
            dashboardVM.DepartmentName = managerDash.FindDepartmentName(dashboardVM.TeamID);
            dashboardVM.ProjectList = managerDash.GetProjectDetails(dashboardVM.TeamID);
            return View(dashboardVM);
        }

        [HttpGet]
        public IActionResult ProjectManagement(int managerID, int teamID, bool message) {

            ViewBag.Created = null;

            if (message) {
                ViewBag.Created = "Specification Created";
            }

            ProjectManagerVM projectVM = new ProjectManagerVM();
            projectVM.ManagerID = managerID;
            projectVM.TeamID = teamID;
            projectVM.Projects = projectManager.GetProject(projectVM.TeamID);
            return View(projectVM);
        }

        [HttpPost]
        public IActionResult ProjectManagement(ProjectManagerVM projectVM, string navOption, string buttonOption) {
            int managerID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            projectVM.ManagerID = managerID;
            switch (navOption) {
                case "dashboard":
                    return RedirectToAction("Index", "Manager",
                        new { managerID = projectVM.ManagerID });
                case "employee":
                    return RedirectToAction("EmployeeManagement", "Manager",
                        new {
                            managerID = projectVM.ManagerID,
                            teamID = projectVM.TeamID
                        });
                case "timesheet":
                    return RedirectToAction("TimesheetDashboard", "Manager",
                        new {
                            managerID = projectVM.ManagerID,
                            teamID = projectVM.TeamID
                        });
                case "profile":
                    return RedirectToAction("Profile", "Manager",
                        new {
                            managerID = projectVM.ManagerID,
                            teamID = projectVM.TeamID
                        });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            switch (buttonOption) {
                case "view":
                    return RedirectToAction("ViewSpecification", "Manager",
                        new {
                            projectID = projectVM.ProjectID,
                            managerID = projectVM.ManagerID
                        });
                case "create":
                    return RedirectToAction("CreateSpecification", "Manager",
                        new {
                            projectID = projectVM.ProjectID,
                            managerID = projectVM.ManagerID,
                            teamID = projectVM.TeamID
                        });
            }

            projectVM.Projects = projectManager.GetProject(projectVM.TeamID);
            return View(projectVM);
        }

        [HttpGet]
        public IActionResult ViewSpecification(int projectID, int managerID) {
            ViewBag.IsNotCompleted = null;
            ViewBag.Completed = null;
            SpecificationVM specificationVM = new SpecificationVM();
            specificationVM.ManagerID = managerID;
            specificationVM.ProjectID = projectID;
            specificationVM.ProjectName = projectManager.GetProjectName(specificationVM.ProjectID);
            specificationVM.Specifications = projectManager.GetSpecifications(specificationVM.ProjectID);
            return View(specificationVM);
        }

        [HttpPost]
        public IActionResult ViewSpecification(SpecificationVM specificationVM, string buttonOption) {
            ViewBag.IsNotCompleted = null;
            ViewBag.Completed = null;
            bool checkCompleteness = false;
            int numSpecifications = 0;
            bool isCompleted = false;

            switch (buttonOption) {
                case "completed":
                    checkCompleteness = true;
                    break;
                case "back":
                    specificationVM.TeamID = managerDash.FindTeamID(specificationVM.ManagerID);
                    specificationVM.TeamName = managerDash.FindTeamName(specificationVM.TeamID);
                    return RedirectToAction("ProjectManagement", "Manager",
                        new {
                            managerID = specificationVM.ManagerID,
                            teamID = specificationVM.TeamID
                        });
            }

            if (checkCompleteness) {
                numSpecifications = projectManager.SpecificationCount(specificationVM.ProjectID);
                isCompleted = projectManager.CheckProjectStatus(specificationVM.ProjectID, numSpecifications);
            }

            if (isCompleted) {
                bool isUpdated = projectManager.UpdateProjectStatus(specificationVM.ProjectID);

                if (!isUpdated) {
                    ViewBag.IsNotCompleted = "An error has occured";
                    specificationVM.ProjectName = projectManager.GetProjectName(specificationVM.ProjectID);
                    specificationVM.Specifications = projectManager.GetSpecifications(specificationVM.ProjectID);
                    return View(specificationVM);
                }

                ViewBag.Completed = "Project has been marked as completed.";
                specificationVM.ProjectName = projectManager.GetProjectName(specificationVM.ProjectID);
                specificationVM.Specifications = projectManager.GetSpecifications(specificationVM.ProjectID);
                return View(specificationVM);
            }

            ViewBag.IsNotCompleted = "Not all specifications are completed.";
            specificationVM.ProjectName = projectManager.GetProjectName(specificationVM.ProjectID);
            specificationVM.Specifications = projectManager.GetSpecifications(specificationVM.ProjectID);
            return View(specificationVM);
        }

        [HttpGet]
        public IActionResult CreateSpecification(int projectID, int managerID, int teamID) {
            ViewBag.Error = null;
            CreateSpecificationVM createSpecificationVM = new CreateSpecificationVM();
            createSpecificationVM.ProjectID = projectID;
            createSpecificationVM.ManagerID = managerID;
            createSpecificationVM.TeamID = teamID;
            return View(createSpecificationVM);
        }

        [HttpPost]
        public IActionResult CreateSpecification(CreateSpecificationVM createSpecificationVM) {
            ViewBag.Error = null;
            // Checking null values
            if (createSpecificationVM.Description.IsNullOrEmpty()) {
                ViewBag.Error = "Description can't be left empty";
                return View(createSpecificationVM);

            }

            if (createSpecificationVM.Deadline == null || createSpecificationVM.Deadline == DateTime.MinValue) {
                ViewBag.Error = "Select or Enter a deadline.";
                return View(createSpecificationVM);

            }

            if (createSpecificationVM.Keyword.IsNullOrEmpty()) {
                ViewBag.Error = "You need to search employees";
                return View(createSpecificationVM);
            }

            return RedirectToAction("SelectEmployeeSpecification", "Manager",
                new {
                    keyword = createSpecificationVM.Keyword,
                    managerID = createSpecificationVM.ManagerID,
                    teamID = createSpecificationVM.TeamID,
                    projectID = createSpecificationVM.ProjectID,
                    description = createSpecificationVM.Description,
                    deadline = createSpecificationVM.Deadline
                });
        }

        [HttpGet]
        public IActionResult SelectEmployeeSpecification(string keyword, int managerID, int teamID, int projectID,
            string description, DateTime deadline) {
            CreateSpecificationVM createSpecificationVM = new CreateSpecificationVM();
            createSpecificationVM.Keyword = keyword;
            createSpecificationVM.ManagerID = managerID;
            createSpecificationVM.TeamID = teamID;
            createSpecificationVM.ProjectID = projectID;
            createSpecificationVM.Description = description;
            createSpecificationVM.Deadline = deadline;
            createSpecificationVM.Employees = projectManager.GetEmployees(createSpecificationVM.TeamID,
                createSpecificationVM.Keyword);
            return View(createSpecificationVM);
        }

        [HttpPost]
        public IActionResult SelectEmployeeSpecification(CreateSpecificationVM createSpecificationVM) {
            bool isCreated = projectManager.CreateSpecification(createSpecificationVM.ProjectID,
                createSpecificationVM.EmployeeID, createSpecificationVM.Description,
                createSpecificationVM.Deadline
                );
            return RedirectToAction("ProjectManagement", "Manager",
                new {
                    managerID = createSpecificationVM.ManagerID,
                    teamID = createSpecificationVM.TeamID,
                    message = isCreated
                });
        }

        public static string GetFullDate(DateTime date) {
            int day = date.Day;
            string suffix = day % 10 == 1 && day != 11 ? "st" :
                            day % 10 == 2 && day != 12 ? "nd" :
                            day % 10 == 3 && day != 13 ? "rd" : "th";

            return $"{date:MMMM} {day}{suffix}, {date:yyyy}";
        }

        [HttpGet]
        public IActionResult TimesheetDashboard(int managerID, int teamID) {

            ViewBag.Error = null;

            ClockInOutVM clock = new ClockInOutVM();
            clock.EmployeeID = managerID;
            clock.TeamID = teamID;
            clock.Workdate = (DateTime.Today);
            clock.DisplayWorkdate = GetFullDate(clock.Workdate);
            clock.Attendances = timesheet.GetAttendances(clock.EmployeeID);
            clock.Workdate = DateTime.Now;
            return View(clock);
        }

        [HttpPost]
        public IActionResult TimesheetDashboard(ClockInOutVM clock, string navOption, string clockButton) {
            // Retrieve the logged-in manager's ID from claims
            int managerID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            clock.EmployeeID = managerID;
            ViewBag.Error = null;

            switch (navOption) {
                case "dashboard":
                    return RedirectToAction("Index", "Manager",
                    new {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "project":
                    return RedirectToAction("ProjectManagement", "Manager",
                    new {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "employee":
                    return RedirectToAction("EmployeeManagement", "Manager",
                    new {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "profile:":
                    return RedirectToAction("Profile", "Manager",
                    new {
                        employeeID = clock.EmployeeID,
                        teamID = clock.TeamID
                    });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            switch (clockButton) {
                case "clockin":
                    clock.Workdate = DateTime.Now;
                    bool checkClockedIn = timesheet.CheckClockIn(clock.EmployeeID, clock.Workdate);
                    if (checkClockedIn) {
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
                    if (checkClockedOut) {
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

        [HttpGet]
        public IActionResult EmployeeManagement(int teamID) {
            int managerID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            EmployeeManagementVM employeeVM = new EmployeeManagementVM();
            employeeVM.ManagerID = managerID;
            employeeVM.TeamID = teamID;
            employeeVM.ManagerName = managerDash.FindManagerName(employeeVM.ManagerID);
            employeeVM.Employees = managerDash.GetEmployees(employeeVM.TeamID);
            return View(employeeVM);
        }

        [HttpPost]
        public IActionResult EmployeeManagement(EmployeeManagementVM employeeVM, string navOption) {
            int managerID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            employeeVM.ManagerID = managerID;
            switch (navOption) {
                case "dashboard":
                    employeeVM.TeamID = managerDash.FindTeamID(employeeVM.ManagerID);
                    return RedirectToAction("Index", "Manager", new {
                        managerID = employeeVM.ManagerID,
                        teamID = employeeVM.TeamID
                    });
                case "project":
                    return RedirectToAction("ProjectManagement", "Manager", new {
                        managerID = employeeVM.ManagerID,
                        teamID = employeeVM.TeamID
                    });
                case "timesheet":
                    return RedirectToAction("TimesheetDashboard", "Manager", new {
                        managerID = employeeVM.ManagerID,
                        teamID = employeeVM.TeamID
                    });
                case "profile":
                    return RedirectToAction("Profile", "Manager",
                        new {
                            managerID = employeeVM.ManagerID,
                            teamID = employeeVM.TeamID
                        });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            employeeVM.ManagerName = managerDash.FindManagerName(employeeVM.ManagerID);
            employeeVM.Employees = managerDash.GetEmployees(employeeVM.ManagerID);
            return View(employeeVM);
        }
    }
}
