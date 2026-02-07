using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller {
        private AdminDepartmentDAL _department;
        private AdminPositionDAL _position;
        private AdminTeamDAL _team;
        private AdminEmployeeDAL _employee;
        private AdminDashboardDAL _dashboard;

        public AdminController() {
            _department = new AdminDepartmentDAL();
            _position = new AdminPositionDAL();
            _team = new AdminTeamDAL();
            _employee = new AdminEmployeeDAL();
            _dashboard = new AdminDashboardDAL();
        }

        [HttpGet]
        public IActionResult Index() {
            // Retrieve the logged-in admin's ID from claims
            int adminID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");

            AdminDashboardViewModel adminDash = new AdminDashboardViewModel();

            adminDash.EmployeeID = adminID;
            adminDash.PolarChart = _dashboard.GetPolarChartInfo();
            adminDash.BarChart = _dashboard.GetBarChartInfo();
            adminDash.Projects = _dashboard.GetProjectList();

            return View(adminDash);
        }

        [HttpPost]
        public IActionResult Index(AdminDashboardViewModel adminDash, string navOption) {
            adminDash.EmployeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");

            switch (navOption) {
                case "department":
                    return RedirectToAction("Department", "Admin", new { adminID = adminDash.EmployeeID });
                case "team":
                    return RedirectToAction("Team", "Admin", new { adminID = adminDash.EmployeeID });
                case "position":
                    return RedirectToAction("Position", "Admin", new { adminID = adminDash.EmployeeID });
                case "employee":
                    return RedirectToAction("Employee", "Admin", new { adminID = adminDash.EmployeeID });
                case "management":
                    return RedirectToAction("Management", "AdminManagement", new { adminID = adminDash.EmployeeID });
                case "profile":
                    return RedirectToAction("Profile", "Admin", new { adminID = adminDash.EmployeeID });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            adminDash.PolarChart = _dashboard.GetPolarChartInfo();
            adminDash.BarChart = _dashboard.GetBarChartInfo();
            adminDash.Projects = _dashboard.GetProjectList();
            return View(adminDash);
        }

        [HttpGet]
        public IActionResult Department(int adminID) {
            DepartmentViewModel department = new DepartmentViewModel();
            department.EmployeeID = adminID;
            department.Departments = _department.GetDepartments();
            return View(department);
        }

        [HttpPost]
        public IActionResult Department(DepartmentViewModel department, string navOption) {
            department.EmployeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            switch (navOption) {
                case "dashboard":
                    return RedirectToAction("Index", "Admin", new { adminID = department.EmployeeID });
                case "team":
                    return RedirectToAction("Team", "Admin", new { adminID = department.EmployeeID });
                case "position":
                    return RedirectToAction("Position", "Admin", new { adminID = department.EmployeeID });
                case "employee":
                    return RedirectToAction("Employee", "Admin", new { adminID = department.EmployeeID });
                case "management":
                    return RedirectToAction("Management", "AdminManagement", new { adminID = department.EmployeeID });
                case "profile":
                    return RedirectToAction("Profile", "Admin", new { adminID = department.EmployeeID });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            department.Departments = _department.GetDepartments();
            return View(department);
        }

        [HttpGet]
        public IActionResult AddDepartment() {
            ViewBag.Error = false;
            ViewBag.NullError = false;
            ViewBag.SpecialCharError = false;
            ViewBag.DuplicateError = false;
            ViewBag.Success = false;
            return View(new Department());
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department) {
            ViewBag.Error = false;
            ViewBag.NullError = false;
            ViewBag.SpecialCharError = false;
            ViewBag.DuplicateError = false;
            ViewBag.Success = false;

            // List of special characters to check
            var specialCharacters = new List<char> { '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '!', '~', '`', '<', '>', '/', '\\', '|', '[', ']', '{', '}', ':', ';', '"', '\'', ',', '.', '?', '_' };

            // Check if the department name is null or empty
            if (string.IsNullOrWhiteSpace(department.DepartmentName)) {
                ViewBag.NullError = true;
            }
            // Check if the department name contains special characters
            else if (department.DepartmentName.Any(ch => specialCharacters.Contains(ch))) {
                ViewBag.SpecialCharError = true;
            } else {
                // Check for duplicate department name
                if (_department.CheckDepartmentName(department.DepartmentName)) {
                    ViewBag.DuplicateError = true; // Set the duplicate error message
                } else {
                    // Try adding the department
                    bool addSuccessful = _department.AddNewDepartment(department.DepartmentName);

                    if (addSuccessful) {
                        ViewBag.Success = true; // Success message
                    } else {
                        ViewBag.Error = true; // Generic add failure message
                    }
                }
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult DeleteDepartment() {
            ViewBag.Error = false;
            ViewBag.Success = false;
            DeleteDepartmentViewModel departments = new DeleteDepartmentViewModel();
            departments.DepartmentList = _department.GetDepartments();
            departments.Departments = departments.DepartmentList.Select(d => new SelectListItem(d.DepartmentName, d.DepartmentID.ToString())).ToList();
            return View(departments); // Return a new Department object for binding  
        }

        [HttpPost]
        public IActionResult DeleteDepartment(DeleteDepartmentViewModel department) {
            ViewBag.Error = false;
            ViewBag.Success = false;
            // Check if no department is selected (DepartmentID is 0 or default)
            if (department.DepartmentID == 0) {
                ViewBag.Error = true;
                department.DepartmentList = _department.GetDepartments(); // Reload the department list
                department.Departments = department.DepartmentList.Select(d =>
                    new SelectListItem(d.DepartmentName, d.DepartmentID.ToString())).ToList();
                return View(department);
            }

            // Try deleting the selected department
            bool isDeleted = _department.DeleteDepartment(department.DepartmentID);

            if (isDeleted) {
                ViewBag.Success = true;
            }

            department.DepartmentList = _department.GetDepartments(); // Reload the department list
            department.Departments = department.DepartmentList.Select(d =>
                new SelectListItem(d.DepartmentName, d.DepartmentID.ToString())).ToList();
            return View(department);
        }

        [HttpGet]
        public IActionResult Position(int adminID) {
            PositionViewModel positionVM = new PositionViewModel();
            positionVM.EmployeeID = adminID;
            positionVM.PositionList = _position.GetAllPositions();
            return View(positionVM);
        }

        [HttpPost]
        public IActionResult Position(PositionViewModel positionVM, string navOption) {
            positionVM.EmployeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");

            switch (navOption) {
                case "dashboard":
                    return RedirectToAction("Index", "Admin", new { adminID = positionVM.EmployeeID });
                case "department":
                    return RedirectToAction("Department", "Admin", new { adminID = positionVM.EmployeeID });
                case "team":
                    return RedirectToAction("Team", "Admin", new { adminID = positionVM.EmployeeID });
                case "employee":
                    return RedirectToAction("Employee", "Admin", new { adminID = positionVM.EmployeeID });
                case "management":
                    return RedirectToAction("Management", "AdminManagement", new { adminID = positionVM.EmployeeID });
                case "profile":
                    return RedirectToAction("Profile", "Admin", new { adminID = positionVM.EmployeeID });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            positionVM.PositionList = _position.GetAllPositions();
            return View(positionVM);
        }

        [HttpGet]
        public IActionResult AddPosition() {
            // Initialize ViewBag for all messages
            ViewBag.IsEmptyError = false;
            ViewBag.IsDuplicateError = false;
            ViewBag.IsSpecialCharError = false;
            ViewBag.IsSuccess = false;

            return View(new Position());
        }

        [HttpPost]
        public IActionResult AddPosition(Position position) {
            // Validate if the position name is empty
            if (string.IsNullOrWhiteSpace(position.PositionName)) {
                ViewBag.IsEmptyError = true;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = false;
                return View(position);
            }

            if (ContainsSpecialCharacters(position.PositionName)) {
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = true;
                ViewBag.IsSuccess = false;
                return View(position);
            }

            // Try adding the position
            bool isAdded = _position.AddPosition(position.PositionName);

            if (!isAdded) {
                // Handle duplicate or failed addition
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = true;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = false;
            } else {
                // Successfully added
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = true;
            }

            return View(position);
        }

        [HttpGet]
        public IActionResult DeletePosition() {
            // Initialize ViewBag for validation and success/error messages
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            // Fetch all positions
            PositionViewModel positionVM = new PositionViewModel();
            positionVM.PositionList = _position.GetAllPositions();

            // Populate dropdown list
            positionVM.Positions = positionVM.PositionList.Select(p => new SelectListItem {
                Text = p.PositionName,
                Value = p.PositionID.ToString()
            }).ToList();

            return View(positionVM);
        }

        [HttpPost]
        public IActionResult DeletePosition(PositionViewModel positionVM) {
            // Initialize ViewBag for validation and success/error messages
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            if (positionVM.PositionID == 0) {
                ViewBag.ErrorMessage = true;
                positionVM.PositionList = _position.GetAllPositions(); // Reload the position list
                positionVM.Positions = positionVM.PositionList.Select(p =>
                    new SelectListItem(p.PositionName, p.PositionID.ToString())).ToList();
                return View(positionVM);
            }

            // Try deleting the selected department
            bool isDeleted = _position.DeletePosition(positionVM.PositionID);

            if (isDeleted) {
                ViewBag.SuccessMessage = true;
            }

            positionVM.PositionList = _position.GetAllPositions(); // Reload the position list
            positionVM.Positions = positionVM.PositionList.Select(p =>
                new SelectListItem(p.PositionName, p.PositionID.ToString())).ToList();
            return View(positionVM);
        }

        [HttpGet]
        public IActionResult Team(int adminID) {
            TeamViewModel teamVM = new TeamViewModel();
            teamVM.EmployeeID = adminID;
            teamVM.TeamList = _team.GetAllTeams();
            return View(teamVM);
        }

        [HttpPost]
        public IActionResult Team(TeamViewModel teamVM, string navOption) {
            teamVM.EmployeeID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            switch (navOption) {
                case "dashboard":
                    return RedirectToAction("Index", "Admin", new { adminID = teamVM.EmployeeID });
                case "department":
                    return RedirectToAction("Department", "Admin", new { adminID = teamVM.EmployeeID });
                case "position":
                    return RedirectToAction("Position", "Admin", new { adminID = teamVM.EmployeeID });
                case "employee":
                    return RedirectToAction("Employee", "Admin", new { adminID = teamVM.EmployeeID });
                case "management":
                    return RedirectToAction("Management", "AdminManagement", new { adminID = teamVM.EmployeeID });
                case "profile":
                    return RedirectToAction("Profile", "Admin", new { adminID = teamVM.EmployeeID });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            teamVM.TeamList = _team.GetAllTeams();
            return View(teamVM);
        }
        [HttpGet]
        public IActionResult AddTeam() {
            // Initialize ViewBag for all messages
            ViewBag.IsEmptyError = false;
            ViewBag.IsDuplicateError = false;
            ViewBag.IsSpecialCharError = false;
            ViewBag.IsSuccess = false;
            ViewBag.IsSelected = false;

            TeamViewModel teamVM = new TeamViewModel();
            teamVM.TeamList = _team.GetAllTeams();
            teamVM.DepartmentList = _department.GetDepartments();
            teamVM.Departments = teamVM.DepartmentList.Select(t =>
                    new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
            return View(teamVM);
        }

        [HttpPost]
        public IActionResult AddTeam(TeamViewModel teamVM) {
            if (string.IsNullOrWhiteSpace(teamVM.TeamName) && teamVM.DepartmentID == 0) {
                ViewBag.IsEmptyError = true;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = false;
                ViewBag.IsSelected = true;
                teamVM.DepartmentList = _department.GetDepartments();
                teamVM.Departments = teamVM.DepartmentList.Select(t =>
                        new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
                return View(teamVM);
            } else if (string.IsNullOrWhiteSpace(teamVM.TeamName)) {
                ViewBag.IsEmptyError = true;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = false;
                ViewBag.IsSelected = false;
                teamVM.DepartmentList = _department.GetDepartments();
                teamVM.Departments = teamVM.DepartmentList.Select(t =>
                        new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
                return View(teamVM);
            }

            if (ContainsSpecialCharacters(teamVM.TeamName) && teamVM.DepartmentID == 0) {
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = true;
                ViewBag.IsSuccess = false;
                ViewBag.IsSelected = true;
                teamVM.DepartmentList = _department.GetDepartments();
                teamVM.Departments = teamVM.DepartmentList.Select(t =>
                        new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
                return View(teamVM);
            } else if (ContainsSpecialCharacters(teamVM.TeamName)) {
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = true;
                ViewBag.IsSuccess = false;
                ViewBag.IsSelected = false;
                teamVM.DepartmentList = _department.GetDepartments();
                teamVM.Departments = teamVM.DepartmentList.Select(t =>
                        new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
                return View(teamVM);
            }

            if (teamVM.DepartmentID == 0) {
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = false;
                ViewBag.IsSelected = true;
                teamVM.DepartmentList = _department.GetDepartments();
                teamVM.Departments = teamVM.DepartmentList.Select(t =>
                        new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
                return View(teamVM);
            }

            bool isAdded = _team.AddTeam(teamVM.TeamName, teamVM.DepartmentID);

            if (!isAdded) {
                // Handle duplicate or failed addition
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = true;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = false;
                ViewBag.IsSelected = true;
                teamVM.DepartmentList = _department.GetDepartments();
                teamVM.Departments = teamVM.DepartmentList.Select(t =>
                        new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
            } else {
                // Successfully added
                ViewBag.IsEmptyError = false;
                ViewBag.IsDuplicateError = false;
                ViewBag.IsSpecialCharError = false;
                ViewBag.IsSuccess = true;
                ViewBag.IsSelected = false;
            }

            teamVM.DepartmentList = _department.GetDepartments();
            teamVM.Departments = teamVM.DepartmentList.Select(t =>
                    new SelectListItem(t.DepartmentName, t.DepartmentID.ToString())).ToList();
            return View(teamVM);
        }

        [HttpGet]
        public IActionResult DeleteTeam() {
            // Initialize ViewBag for validation and success/error messages
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            TeamViewModel teamVM = new TeamViewModel();
            teamVM.TeamList = _team.GetAllTeams();
            teamVM.Teams = teamVM.TeamList.Select(t => new SelectListItem {
                Text = t.TeamName,
                Value = t.TeamID.ToString()
            }).ToList();

            return View(teamVM);
        }

        [HttpPost]
        public IActionResult DeleteTeam(TeamViewModel teamVM) {
            // Initialize ViewBag for validation and success/error messages
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            if (teamVM.TeamID == 0) {
                ViewBag.ErrorMessage = true;
                teamVM.TeamList = _team.GetAllTeams();
                teamVM.Teams = teamVM.TeamList.Select(t => new SelectListItem {
                    Text = t.TeamName,
                    Value = t.TeamID.ToString()
                }).ToList();

                return View(teamVM);
            }

            // Try deleting the selected department
            bool isDeleted = _team.DeleteTeam(teamVM.TeamID);

            if (isDeleted) {
                ViewBag.SuccessMessage = true;
            }

            teamVM.TeamList = _team.GetAllTeams();
            teamVM.Teams = teamVM.TeamList.Select(t => new SelectListItem {
                Text = t.TeamName,
                Value = t.TeamID.ToString()
            }).ToList();

            return View(teamVM);
        }

        [HttpGet]
        public IActionResult Employee(int adminID) {
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            string manager = null;
            employeeVM.AdminID = adminID;
            employeeVM.EmployeeList = _employee.GetEmployeeDash();

            foreach (var employee in employeeVM.EmployeeList) {
                if (employee.IsManager == "no") {
                    manager = _employee.GetManager(employee.TeamID);

                    if (manager != null) {
                        employee.Manager = manager;
                    }
                }
            }

            return View(employeeVM);
        }

        [HttpPost]
        public IActionResult Employee(EmployeeViewModel employeeVM, string navOption) {
            employeeVM.AdminID = int.Parse(User.FindFirst("EmployeeID")?.Value ?? "0");
            switch (navOption) {
                case "dashboard":
                    return RedirectToAction("Index", "Admin", new { adminID = employeeVM.AdminID });
                case "department":
                    return RedirectToAction("Department", "Admin", new { adminID = employeeVM.AdminID });
                case "team":
                    return RedirectToAction("Team", "Admin", new { adminID = employeeVM.AdminID });
                case "position":
                    return RedirectToAction("Position", "Admin", new { adminID = employeeVM.AdminID });
                case "management":
                    return RedirectToAction("Management", "AdminManagement", new { adminID = employeeVM.AdminID });
                case "profile":
                    return RedirectToAction("Profile", "Admin", new { adminID = employeeVM.AdminID });
                case "logout":
                    return RedirectToAction("Logout", "Login");
            }

            string manager = null;
            employeeVM.EmployeeList = _employee.GetEmployeeDash();
            foreach (var employee in employeeVM.EmployeeList) {
                if (employee.IsManager == "no") {
                    manager = _employee.GetManager(employee.TeamID);

                    if (manager != null) {
                        employee.Manager = manager;
                    }
                }
            }
            return View(employeeVM);
        }
        static bool IsValid(string emailaddress) {
            string domain = "@gmail.com";

            if (emailaddress.Substring(emailaddress.Length - domain.Length) != domain) {
                return false;
            }

            try {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            } catch (FormatException) {
                return false;
            }
        }

        [HttpGet]
        public IActionResult AddEmployee() {

            ViewBag.Error = null;
            ViewBag.Success = null;

            EmployeeDetailsViewModel employeeDetailsVM = new EmployeeDetailsViewModel();

            employeeDetailsVM.PositionList = _position.GetAllPositions();
            employeeDetailsVM.RoleList = _employee.GetRoles();

            employeeDetailsVM.Positions = employeeDetailsVM.PositionList.Select(p => new SelectListItem {
                Text = p.PositionName,
                Value = p.PositionID.ToString()
            }).ToList();

            employeeDetailsVM.Roles = employeeDetailsVM.RoleList.Select(r => new SelectListItem {
                Text = r.RoleName,
                Value = r.RoleID.ToString()
            }).ToList();

            return View(employeeDetailsVM);
        }

        public void AddEmployeeReload(EmployeeDetailsViewModel employeeDetailsVM) {
            employeeDetailsVM.PositionList = _position.GetAllPositions();
            employeeDetailsVM.RoleList = _employee.GetRoles();
            employeeDetailsVM.Positions = employeeDetailsVM.PositionList.Select(p => new SelectListItem {
                Text = p.PositionName,
                Value = p.PositionID.ToString()
            }).ToList();

            employeeDetailsVM.Roles = employeeDetailsVM.RoleList.Select(r => new SelectListItem {
                Text = r.RoleName,
                Value = r.RoleID.ToString()
            }).ToList();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDetailsViewModel employeeDetailsVM) {
            ViewBag.Error = null;
            ViewBag.Success = null;

            // Validation for input fields
            if (ContainsSpecialCharacters(employeeDetailsVM.FirstName)) {
                ViewBag.Error = "Firstname contains invalid characters.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            if (ContainsSpecialCharacters(employeeDetailsVM.LastName)) {
                ViewBag.Error = "Lastname contains invalid characters.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            if (ContainsSpecialCharacters(employeeDetailsVM.City)) {
                ViewBag.Error = "City contains invalid characters.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            if (ContainsSpecialCharacters(employeeDetailsVM.Street)) {
                ViewBag.Error = "Street contains invalid characters.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            // Salary validation
            if (!int.TryParse(employeeDetailsVM.Salary.ToString(), out int salary) || salary <= 0) {
                ViewBag.Error = "Salary must be a valid positive number.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            // Phone number validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(employeeDetailsVM.Phone.ToString(), @"^5\d{7}$")) {
                ViewBag.Error = "Phone number must start with '5' followed by 7 digits.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            if (employeeDetailsVM.Email.IsNullOrEmpty()) {
                ViewBag.Error = "The email field cannot be empty.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }


            bool isUnique = _employee.CheckEmailUniqueness(employeeDetailsVM.Email);

            bool isValid = false;

            if (isUnique) {
                isValid = IsValid(employeeDetailsVM.Email);
            }

            if (!isValid) {
                ViewBag.Error = "Email must follow the format: example@gmail.com";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            // Position validation
            if (employeeDetailsVM.PositionID == 0) {
                ViewBag.Error = "Please select a valid position.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            // Role validation
            if (employeeDetailsVM.RoleID == 0) {
                ViewBag.Error = "Please select a valid role.";
                AddEmployeeReload(employeeDetailsVM);
                return View(employeeDetailsVM);
            }

            // Attempt to add employee
            bool isEmployeeAdded = _employee.AddEmployee(
                employeeDetailsVM.FirstName,
                employeeDetailsVM.LastName,
                employeeDetailsVM.Email,
                employeeDetailsVM.Phone,
                employeeDetailsVM.City,
                employeeDetailsVM.Street,
                employeeDetailsVM.DateOfBirth,
                employeeDetailsVM.Salary,
                employeeDetailsVM.PositionID
            );


            string username = CreateUsername(employeeDetailsVM);
            employeeDetailsVM.Username = username;
            string password = CreatePassword(employeeDetailsVM);
            employeeDetailsVM.Password = password;

            employeeDetailsVM.EmployeeID = _employee.GetEmployeeID(employeeDetailsVM.Email);

            if (employeeDetailsVM.EmployeeID != 0) {
                bool isCreate = _employee.CreateUser(
                    employeeDetailsVM.Username,
                    employeeDetailsVM.Password,
                    employeeDetailsVM.EmployeeID,
                    employeeDetailsVM.RoleID
                );

                if (isEmployeeAdded && isCreate) {
                    ViewBag.Success = "Employee added successfully.";
                    AddEmployeeReload(employeeDetailsVM);
                    return View(employeeDetailsVM); // Reset form after successful submission
                }
            }

            ViewBag.Error = "Failed to add employee. Please try again.";
            AddEmployeeReload(employeeDetailsVM);
            return View(employeeDetailsVM);
        }

        public IActionResult ViewAllEmployees() {
            EmployeeAllViewModel employeeAllVM = new EmployeeAllViewModel();
            employeeAllVM.Employees = _employee.GetEmployees();
            return View(employeeAllVM);
        }

        public void DeleteReloadList(DeleteEmployeeViewModel employeeDeleteVM) {
            employeeDeleteVM.EmployeeNames = _employee.GetEmployeeNames();
            employeeDeleteVM.FullNames = employeeDeleteVM.EmployeeNames.Select(ed => new SelectListItem { Text = ed.FullName, Value = ed.EmployeeID.ToString() }).ToList();
        }

        [HttpGet]
        public IActionResult DeleteEmployee(bool message) {
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            if (message) {
                ViewBag.SuccessMessage = message;
            } else {
                ViewBag.ErrorMessage = message;
            }

            DeleteEmployeeViewModel employeeDeleteVM = new DeleteEmployeeViewModel();
            return View(employeeDeleteVM);
        }

        [HttpPost]
        public IActionResult DeleteEmployee(DeleteEmployeeViewModel employeeDeleteVM) {
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;
            return RedirectToAction("DeleteEmployeeSelect", "Admin", new { keyword = employeeDeleteVM.Keyword });
        }

        [HttpGet]
        public IActionResult DeleteEmployeeSelect(string keyword) {
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;
            DeleteEmployeeViewModel employeeDeleteVM = new DeleteEmployeeViewModel();
            employeeDeleteVM.EmployeeNames = _employee.GetEmployeeByKeyword(keyword);
            employeeDeleteVM.FullNames = employeeDeleteVM.EmployeeNames.Select(ed => new SelectListItem { Text = ed.FullName, Value = ed.EmployeeID.ToString() }).ToList();
            return View(employeeDeleteVM);
        }

        [HttpPost]
        public IActionResult DeleteEmployeeSelect(DeleteEmployeeViewModel employeeDeleteVM) {
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            int employeeID = employeeDeleteVM.EmployeeID;
            bool isDeleted = _employee.DeleteEmployee(employeeID);

            return RedirectToAction("DeleteEmployee", "Admin", new { message = isDeleted });
        }

        [HttpGet]
        public IActionResult SearchEmployeeToUpdate(bool message) {
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;

            if (message) {
                ViewBag.SuccessMessage = message;
            } else {
                ViewBag.ErrorMessage = message;
            }
            UpdateEmployeeViewModel employeeToUpdateVM = new UpdateEmployeeViewModel();
            return View(employeeToUpdateVM);
        }

        [HttpPost]
        public IActionResult SearchEmployeeToUpdate(UpdateEmployeeViewModel employeeToUpdateVM) {
            ViewBag.ErrorMessage = false;
            ViewBag.SuccessMessage = false;
            return RedirectToAction("SelectEmployeeToUpdate", "Admin", new { keyword = employeeToUpdateVM.Keyword });
        }

        [HttpGet]
        public IActionResult SelectEmployeeToUpdate(string keyword) {
            UpdateEmployeeViewModel employeeToUpdateVM = new UpdateEmployeeViewModel();
            employeeToUpdateVM.EmployeeDetails = _employee.GetEmployeeByKeywordForUpdate(keyword);
            employeeToUpdateVM.EmployeeNames = employeeToUpdateVM.EmployeeDetails.Select(ed => new SelectListItem { Text = ed.FullName, Value = ed.EmployeeID.ToString() }).ToList();
            return View(employeeToUpdateVM);
        }

        [HttpPost]
        public IActionResult SelectEmployeeToUpdate(UpdateEmployeeViewModel employeeToUpdateVM) {
            return RedirectToAction("ChooseUpdateOption", "Admin", new { employeeID = employeeToUpdateVM.EmployeeID });
        }

        [HttpGet]
        public IActionResult ChooseUpdateOption(int employeeID) {
            UpdateEmployeeViewModel employeeToUpdateVM = new UpdateEmployeeViewModel();
            employeeToUpdateVM.EmployeeID = employeeID;
            employeeToUpdateVM.FullName = _employee.GetEmployeeName(employeeID);
            return View(employeeToUpdateVM);
        }

        [HttpPost]
        public IActionResult ChooseUpdateOption(UpdateEmployeeViewModel employeeToUpdateVM, string updateOption) {
            switch (updateOption) {
                case "Name":
                    return RedirectToAction("UpdateName", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                case "Email":
                    return RedirectToAction("UpdateEmail", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                case "DateOfBirth":
                    return RedirectToAction("UpdateDOB", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                case "PhoneNumber":
                    return RedirectToAction("UpdatePhoneNumber", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                case "Address":
                    return RedirectToAction("UpdateAddress", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                case "Salary":
                    return RedirectToAction("UpdateSalary", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                case "Position":
                    return RedirectToAction("SearchEmployeePositionToUpdate", "AdminUpdate", new { employeeID = employeeToUpdateVM.EmployeeID });
                default:
                    return View(employeeToUpdateVM);
            }

        }
        private bool ContainsSpecialCharacters(string input) {
            if (string.IsNullOrWhiteSpace(input)) return true;

            // Define a regex pattern for allowed characters (letters, numbers, spaces)
            string pattern = @"^[a-zA-Z0-9\s]+$";
            return !System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
        }
        private string CreateUsername(EmployeeDetailsViewModel employeeDetailsVM) {
            string username = (employeeDetailsVM.FirstName) + (employeeDetailsVM.DateOfBirth.Day.ToString())
                + (employeeDetailsVM.DateOfBirth.Month.ToString());
            return username;
        }
        private string CreatePassword(EmployeeDetailsViewModel employeeVM) {
            string firstCharacter = employeeVM.FirstName.Substring(0, 1);
            string secondCharacter = employeeVM.LastName.Substring(0, 1);
            string remaining = employeeVM.Username.Substring(employeeVM.Username.Length - 4);
            string password = firstCharacter + secondCharacter + "@" + remaining;

            return password;
        }
    }
}
