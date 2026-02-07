using EmployeeManagementSystem.ViewModels.TeamManagementVM;
using EmployeeManagementSystem.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminManagementController : Controller {
        private AdminManagementDAL management;

        public AdminManagementController() {
            management = new AdminManagementDAL();
        }

        [HttpGet]
        public IActionResult Management(bool message, int adminID) {
            ViewBag.Error = null;
            ViewBag.Success = null;

            if (message) {
                ViewBag.Success = "Manager Successfully Assigned.";
            }

            TeamManagementViewModel teamVM = new TeamManagementViewModel();
            teamVM.AdminID = adminID;
            return View(teamVM);
        }

        [HttpPost]
        public IActionResult Management(TeamManagementViewModel teamVM, string managementOption, string navOption) {
            ViewBag.Error = null;
            ViewBag.Success = null;

            teamVM.AdminID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");

            switch (navOption) {
                case "department":
                    return RedirectToAction("Department", "Admin", new { adminID = teamVM.AdminID });
                case "team":
                    return RedirectToAction("Team", "Admin", new { adminID = teamVM.AdminID });
                case "position":
                    return RedirectToAction("Position", "Admin", new { adminID = teamVM.AdminID });
                case "employee":
                    return RedirectToAction("Employee", "Admin", new { adminID = teamVM.AdminID });
                case "dashboard":
                    return RedirectToAction("Index", "Admin", new { adminID = teamVM.AdminID });
                case "profile":
                    return RedirectToAction("Profile", "Admin", new { adminID = teamVM.AdminID });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            switch (managementOption) {
                case "AssignManager":
                    return RedirectToAction("SelectTeamManagement", "AdminManagement");
                case "AssignEmployee":
                    return RedirectToAction("SelectTeamForEmployees", "AdminManagement");
                case "AssignTeam":
                    return RedirectToAction("SelectTeamForDept", "AdminManagement");
                case "Project":
                    return RedirectToAction("ProjectDashboard", "Project");
                case "Report":
                    return RedirectToAction("ReportManagement", "AdminManagement");
            }

            return View(teamVM);
        }

        [HttpGet]
        public IActionResult SelectTeamManagement() {
            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            teamManagementViewModel.TeamSelectList = management.GetTeams();
            teamManagementViewModel.Teams = teamManagementViewModel.TeamSelectList.Select(
                t => new SelectListItem {
                    Text = t.TeamName,
                    Value = t.TeamID.ToString()
                }).ToList();
            return View(teamManagementViewModel);
        }

        [HttpPost]
        public IActionResult SelectTeamManagement(TeamManagementViewModel teamManagementViewModel) {
            ViewBag.Error = null;
            ViewBag.Success = null;
            return RedirectToAction("SelectManagerTeamManagement", "AdminManagement",
                new { teamID = teamManagementViewModel.TeamID });
        }

        [HttpGet]
        public IActionResult SelectManagerTeamManagement(int teamID) {
            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            teamManagementViewModel.TeamID = teamID;
            teamManagementViewModel.ManagerSelectList = management.GetManagers();
            teamManagementViewModel.Managers = teamManagementViewModel.ManagerSelectList.Select(
                m => new SelectListItem {
                    Text = m.FullName,
                    Value = m.EmployeeID.ToString()
                }).ToList();
            return View(teamManagementViewModel);
        }

        [HttpPost]
        public IActionResult SelectManagerTeamManagement(TeamManagementViewModel teamManagementViewModel) {
            bool isAssigned = management.AssignManager(teamManagementViewModel.EmployeeID,
                teamManagementViewModel.TeamID);
            return RedirectToAction("Management", "AdminManagement",
                new { message = isAssigned });
        }

        [HttpGet]
        public IActionResult SelectTeamForEmployees() {
            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            teamManagementViewModel.TeamSelectList = management.GetTeamsManagers();
            teamManagementViewModel.Teams = teamManagementViewModel.TeamSelectList.Select(
                t => new SelectListItem {
                    Text = t.TeamName,
                    Value = t.TeamID.ToString()
                }).ToList();
            return View(teamManagementViewModel);
        }

        [HttpPost]
        public IActionResult SelectTeamForEmployees(TeamManagementViewModel teamManagementViewModel) {
            bool msg = false;
            return RedirectToAction("ChooseEmployees", "AdminManagement",
                new {
                    teamID = teamManagementViewModel.TeamID,
                    message = msg
                });
        }

        [HttpGet]
        public IActionResult ChooseEmployees(int teamID, bool message) {
            ViewBag.Error = null;
            ViewBag.Success = null;

            if (message) {
                ViewBag.Success = "Employee has been added";
            }

            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            teamManagementViewModel.TeamID = teamID;
            teamManagementViewModel.TeamName = management.GetTeamName(teamManagementViewModel.TeamID);
            teamManagementViewModel.EmployeeSelectList = management.GetEmployees();
            teamManagementViewModel.Employees = teamManagementViewModel.EmployeeSelectList.Select(
                e => new SelectListItem {
                    Text = e.FullName,
                    Value = e.EmployeeID.ToString()
                }).ToList();
            return View(teamManagementViewModel);
        }

        [HttpPost]
        public IActionResult ChooseEmployees(TeamManagementViewModel teamManagementViewModel) {
            ViewBag.Error = null;
            ViewBag.Success = null;
            bool isEmployeeAssigned = management.AddEmployeeTeam(teamManagementViewModel.EmployeeID,
                teamManagementViewModel.TeamID);
            return RedirectToAction("ChooseEmployees", "AdminManagement",
                new {
                    teamID = teamManagementViewModel.TeamID,
                    message = isEmployeeAssigned
                });
        }

        [HttpGet]
        public IActionResult SelectTeamForDept(bool message) {
            ViewBag.Success = null;

            if (message) {
                ViewBag.Success = "Team Added Suceesfully";
            }

            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            teamManagementViewModel.TeamSelectList = management.GetTeamsWithNoDept();
            teamManagementViewModel.Teams = teamManagementViewModel.TeamSelectList.Select(
                t => new SelectListItem {
                    Text = t.TeamName,
                    Value = t.TeamID.ToString()
                }).ToList();
            return View(teamManagementViewModel);
        }

        [HttpPost]
        public IActionResult SelectTeamForDept(TeamManagementViewModel teamManagementViewModel) {
            return RedirectToAction("SelectDeptForTeam", "AdminManagement",
                new { teamID = teamManagementViewModel.TeamID });
        }

        [HttpGet]
        public IActionResult SelectDeptForTeam(int teamID) {
            TeamManagementViewModel teamManagementViewModel = new TeamManagementViewModel();
            teamManagementViewModel.TeamID = teamID;
            teamManagementViewModel.TeamSelectList = management.GetDepartments();
            teamManagementViewModel.Departments = teamManagementViewModel.TeamSelectList.Select(
                d => new SelectListItem {
                    Text = d.DepartmentName,
                    Value = d.DepartmentID.ToString()
                }).ToList();
            return View(teamManagementViewModel);
        }

        [HttpPost]
        public IActionResult SelectDeptForTeam(TeamManagementViewModel teamManagementViewModel) {
            bool isTeamAdded = management.AddTeamToDept(teamManagementViewModel.TeamID,
                teamManagementViewModel.DepartmentID);
            return RedirectToAction("SelectTeamForDept", "AdminManagement",
                new { message = isTeamAdded });
        }

        [HttpGet]
        public IActionResult ReportManagement() {
            return View();
        }

        [HttpPost]
        public IActionResult ReportManagement(string reportOption) {

            switch (reportOption) {
                case "employee":
                    return RedirectToAction("EmployeeReport", "Report");
                case "employee-detail":
                    return RedirectToAction("SearchEmployee", "EmployeeDetail");
            }
            return View();
        }
    }
}
