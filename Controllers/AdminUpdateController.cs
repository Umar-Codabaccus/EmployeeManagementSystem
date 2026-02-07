using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;

namespace EmployeeManagementSystem.Controllers
{
    public class AdminUpdateController : Controller
    {
        private AdminUpdateDAL _update;
        private AdminEmployeeDAL _employee;

        public AdminUpdateController()
        {
            _update = new AdminUpdateDAL();
            _employee = new AdminEmployeeDAL();
        }

        private bool ContainsSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true;

            // Define a regex pattern for allowed characters (letters, numbers, spaces)
            string pattern = @"^[a-zA-Z0-9\s]+$";
            return !System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
        }

        [HttpGet]
        public IActionResult UpdateName(int employeeID)
        {
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.EmployeeID = employeeID;
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult UpdateName(UpdateEmployeeViewModel empUpdate)
        {
            // Validation for input fields
            if (ContainsSpecialCharacters(empUpdate.FirstName))
            {
                ViewBag.Error = "Firstname contains invalid characters.";
                return View(empUpdate);
            }

            if (ContainsSpecialCharacters(empUpdate.LastName))
            {
                ViewBag.Error = "Lastname contains invalid characters.";
                return View(empUpdate);
            }

            bool isNameUpdated = _update.UpdateEmployeeName(empUpdate.EmployeeID, empUpdate.FirstName, empUpdate.LastName);
            

            if (isNameUpdated)
            {
                return RedirectToAction("UpdateEmail", "AdminUpdate", new { employeeID = empUpdate.EmployeeID});
            }

            return RedirectToAction("SearchEmployeeToUpdate", "Admin", new { message = isNameUpdated});
        }

        [HttpGet]
        public IActionResult UpdateEmail(int employeeID)
        {
            ViewBag.Error = null;
            ViewBag.Success = null;
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.EmployeeID = employeeID;
            return View(empUpdate);
        }

        static bool IsValid(string emailaddress)
        {
            string domain = "@gmail.com";

            if (emailaddress.Substring(emailaddress.Length - domain.Length) != domain)
            {
                return false;
            }

            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult UpdateEmail(UpdateEmployeeViewModel empUpdate)
        {

            if (empUpdate.Email.IsNullOrEmpty())
            {
                ViewBag.Error = "The email field cannot be empty.";
                return View(empUpdate);
            }

            bool isUnique = _employee.CheckEmailUniqueness(empUpdate.Email);

            bool isValid = false;

            if (isUnique)
            {
                isValid = IsValid(empUpdate.Email);
            }

            if (!isValid)
            {
                ViewBag.Error = "Email must follow the format: example@gmail.com";
                return View(empUpdate);
            }

            bool isEmailUpdated = false;

            isEmailUpdated = _update.UpdateEmployeeEmail(empUpdate.EmployeeID, empUpdate.Email);

            if (isEmailUpdated)
            {
                return RedirectToAction("SearchEmployeeToUpdate", "Admin", new { message = isEmailUpdated });
            }

            return View(empUpdate);
        }

        [HttpGet]
        public IActionResult UpdateDOB()
        {
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult UpdateDOB(UpdateEmployeeViewModel empUpdate)
        {
            return View(empUpdate);
        }

        [HttpGet]
        public IActionResult UpdatePhoneNumber(int employeeID)
        {
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.EmployeeID = employeeID;
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult UpdatePhoneNumber(UpdateEmployeeViewModel empUpdate)
        {
            // Phone number validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(empUpdate.Phone.ToString(), @"^5\d{7}$"))
            {
                ViewBag.Error = "Phone number must start with '5' followed by 7 digits.";
                return View(empUpdate);
            }

            bool isPhoneUpdated = _update.UpdateEmployeePhoneNumber(empUpdate.EmployeeID, empUpdate.Phone);

            if (isPhoneUpdated)
            {
                return RedirectToAction("SearchEmployeeToUpdate", "Admin", new { message = isPhoneUpdated});
            }

            return View(empUpdate);
        }

        [HttpGet]
        public IActionResult UpdateAddress(int employeeID)
        {
            ViewBag.Error = null;
            ViewBag.Success = null;
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.EmployeeID = employeeID;
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult UpdateAddress(UpdateEmployeeViewModel empUpdate)
        {
            ViewBag.Error = null;
            ViewBag.Success = null;

            if (ContainsSpecialCharacters(empUpdate.City))
            {
                ViewBag.Error = "City contains invalid characters.";
                return View(empUpdate);
            }

            if (ContainsSpecialCharacters(empUpdate.Street))
            {
                ViewBag.Error = "Street contains invalid characters.";
                return View(empUpdate);
            }

            bool isAddressUpdated = _update.UpdateEmployeeAddress(empUpdate.EmployeeID, empUpdate.City, empUpdate.Street);

            if (isAddressUpdated)
            {
                return RedirectToAction("SearchEmployeeToUpdate", "Admin", new { message = isAddressUpdated} );
            }

            return View(empUpdate);
        }

        [HttpGet]
        public IActionResult UpdateSalary(int employeeID)
        {
            ViewBag.Error = null;
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.EmployeeID = employeeID;
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult UpdateSalary(UpdateEmployeeViewModel empUpdate)
        {
            ViewBag.Error = null;

            // Salary validation
            if (!int.TryParse(empUpdate.Salary.ToString(), out int salary) || salary <= 0)
            {
                ViewBag.Error = "Salary must be a valid positive number.";
                return View(empUpdate);
            }

            bool isSalaryUpdated = false;

            isSalaryUpdated = _update.UpdateEmployeeSalary(empUpdate.EmployeeID, empUpdate.Salary);

            if (isSalaryUpdated)
            {
                return RedirectToAction("SearchEmployeeToUpdate", "Admin", new { message = isSalaryUpdated});
            }
            return View(empUpdate);
        }

        [HttpGet]
        public IActionResult SearchEmployeePositionToUpdate(int employeeID)
        {
            ViewBag.ErrorMessage = false;
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.EmployeeID = employeeID;
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult SearchEmployeePositionToUpdate(UpdateEmployeeViewModel empUpdate)
        {
            ViewBag.ErrorMessage = false;

            if (empUpdate.Keyword.IsNullOrEmpty())
            {
                ViewBag.ErrorMessage = true;
                return View(empUpdate);
            }


            return RedirectToAction("UpdatePosition", "AdminUpdate", new { employeeID = empUpdate.EmployeeID, keyword = empUpdate.Keyword});
        }

        [HttpGet]
        public IActionResult UpdatePosition(int employeeID, string keyword)
        {
            UpdateEmployeeViewModel empUpdate = new UpdateEmployeeViewModel();
            empUpdate.Keyword = keyword;
            empUpdate.EmployeeID = employeeID;
            empUpdate.EmployeeDetails = _update.GetPositionByKeywordForUpdate(empUpdate.Keyword);
            empUpdate.Positions = empUpdate.EmployeeDetails.Select(p => new SelectListItem { Text = p.PositionName, Value = p.PositionID.ToString() }).ToList();
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult UpdatePosition(UpdateEmployeeViewModel empUpdate)
        {
            //empUpdate.PositionID = positionID;
            bool isPositionUpdated = _update.UpdateEmployeePosition(empUpdate.EmployeeID, empUpdate.PositionID);

            if (isPositionUpdated)
            {
                return RedirectToAction("UpdateSalary", "AdminUpdate", new { employeeID = empUpdate.EmployeeID});
            }

            return View(empUpdate);
        }
    }
}
