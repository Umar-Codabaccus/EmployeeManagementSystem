using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.ViewModels.ReportManagement;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers {
    public class EmployeeDetailController : Controller {
        private EmployeeDetailDAL employee;

        public EmployeeDetailController() {
            employee = new EmployeeDetailDAL();
        }

        [HttpGet]
        public IActionResult SearchEmployee() {
            EmployeeDetailVM employeeDetailVM = new EmployeeDetailVM();
            return View(employeeDetailVM);
        }

        [HttpPost]
        public IActionResult SearchEmployee(EmployeeDetailVM employeeDetailVM) {
            return RedirectToAction("DisplayEmployeeList", "EmployeeDetail",
                new { keyword = employeeDetailVM.Keyword });
        }

        [HttpGet]
        public IActionResult DisplayEmployeeList(string keyword) {
            EmployeeDetailVM employeeDetailVM = new EmployeeDetailVM();
            employeeDetailVM.Employees = employee.GetEmployees(keyword);
            return View(employeeDetailVM);
        }

        [HttpPost]
        public IActionResult DisplayEmployeeList(EmployeeDetailVM employeeDetailVM) {

            return RedirectToAction("DetailDashboard", "EmployeeDetail",
                new { employeeID = employeeDetailVM.EmployeeID });
        }

        [HttpGet]
        public IActionResult DetailDashboard(int employeeID) {
            EmployeeDetailVM employeeDetailVM = new EmployeeDetailVM();
            employeeDetailVM = employee.GetEmployee(employeeID);
            return View(employeeDetailVM);
        }
    }
}
