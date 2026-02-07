using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.ManagerModels.Project;
using EmployeeManagementSystem.ViewModels.ProjectManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Controllers {
    public class ProjectController : Controller {
        private AdminDashboardDAL _dashboard;
        private AdminProjectDAL _project;

        public ProjectController() {
            _dashboard = new AdminDashboardDAL();
            _project = new AdminProjectDAL();
        }

        private bool ContainsSpecialCharacters(string input) {
            if (string.IsNullOrWhiteSpace(input)) return true;

            // Define a regex pattern for allowed characters (letters, numbers, spaces)
            string pattern = @"^[a-zA-Z0-9\s]+$";
            return !System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
        }

        public IActionResult ProjectDashboard(bool message) {
            ViewBag.ProjectCreated = false;

            if (message) {
                ViewBag.ProjectCreated = true;
            }
            ProjectDash projectDash = new ProjectDash();
            projectDash.ProjectList = _dashboard.GetProjectList();
            return View(projectDash);
        }

        [HttpGet]
        public IActionResult CreateProject() {
            ViewBag.IsEmptyError = false;
            ViewBag.IsSpecialCharError = false;
            ViewBag.IsDepartmentSearch = null;
            ViewBag.SearchError = null;

            ProjectDash projectDash = new ProjectDash();
            return View(projectDash);
        }

        [HttpPost]
        public IActionResult CreateProject(ProjectDash projectDash) {
            ViewBag.IsEmptyError = false;
            ViewBag.IsSpecialCharError = false;
            ViewBag.IsDepartmentSearch = null;
            ViewBag.SearchError = null;

            if (projectDash.ProjectName.IsNullOrEmpty()) {
                ViewBag.IsEmptyError = true;
                return View(projectDash);
            }

            if (ContainsSpecialCharacters(projectDash.ProjectName)) {
                ViewBag.IsSpecialCharError = true;
                return View(projectDash);
            }

            if (!projectDash.Keyword.IsNullOrEmpty()) {
                projectDash.DepartmentID = _project.GetDepartmentByKeyword(projectDash.Keyword);

                if (projectDash.DepartmentID == 0) {
                    ViewBag.SearchError = "Department Not Found";
                    return View(projectDash);
                }
                return RedirectToAction("SelectTeam", "Project", new {
                    departmentID = projectDash.DepartmentID,
                    projectName = projectDash.ProjectName,
                    deadline = projectDash.Deadline
                });
            } else {
                ViewBag.IsDepartmentSearch = "Enter a department";
            }

            return View(projectDash);
        }

        [HttpGet]
        public IActionResult SelectTeam(int departmentID, string projectName, DateTime deadline) {
            ProjectDash projectDash = new ProjectDash();
            projectDash.DepartmentID = departmentID;
            projectDash.ProjectName = projectName;
            projectDash.Deadline = deadline;
            projectDash.TeamList = _project.GetTeamsByKeyword(projectDash.DepartmentID);
            return View(projectDash);
        }

        [HttpPost]
        public IActionResult SelectTeam(ProjectDash projectDash) {
            bool isProjectCreated = _project.CreateNewProject(projectDash.ProjectName,
                projectDash.TeamID, projectDash.Deadline);

            return RedirectToAction("ProjectDashboard", "Project", new { message = isProjectCreated });
        }
    }
}
