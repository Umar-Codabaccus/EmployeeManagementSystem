using EmployeeManagementSystem.DAL.ReportDAL;
using EmployeeManagementSystem.ViewModels.ReportManagement;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EmployeeManagementSystem.Controllers
{
    public class ReportController : Controller
    {
        private EmployeeReportDAL report;

        public ReportController()
        {
            report = new EmployeeReportDAL();
        }

        [HttpGet]
        public IActionResult EmployeeReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EmployeeReport(string reportOption)
        {
            if (reportOption.Substring(0, 1) == "p")
            {
                return RedirectToAction("SearchPosition", "Report",
                        new { option = reportOption });
            }
            else if (reportOption.Substring(0, 1) == "d")
            {
                return RedirectToAction("SearchDepartment", "Report",
                       new { option = reportOption });
            }

            return View();
        }

        [HttpGet]
        public IActionResult SearchDepartment(string option)
        {
            EmployeeReportVM empReportVM = new EmployeeReportVM();
            empReportVM.Option = option;
            return View(empReportVM);
        }

        [HttpPost]
        public IActionResult SearchDepartment(EmployeeReportVM empReportVM)
        {
            return RedirectToAction("DisplayDepartments", "Report",
                new { option = empReportVM.Option, keyword = empReportVM.Keyword });
        }

        [HttpGet]
        public IActionResult DisplayDepartments(string option, string keyword)
        {
            EmployeeReportVM empReportVM = new EmployeeReportVM();
            empReportVM.Option = option;
            empReportVM.Keyword = keyword;

            empReportVM.Departments = report.GetDepartments(empReportVM.Keyword);
            return View(empReportVM);
        }

        [HttpPost]
        public IActionResult DisplayDepartments(EmployeeReportVM empReportVM)
        {
            string sort = "";
            switch (empReportVM.Option)
            {
                case "department":
                    sort = "department-none";
                    return RedirectToAction("DisplayEmployeeDepartments", "Report",
                        new
                        {
                            departmentID = empReportVM.DepartmentID,
                            option = sort
                        });
                case "department-name-asc":
                    sort = "department-name-asc";
                    return RedirectToAction("DisplayEmployeeDepartments", "Report",
                        new
                        {
                            departmentID = empReportVM.DepartmentID,
                            option = sort
                        });
                case "department-name-desc":
                    sort = "department-name-desc";
                    return RedirectToAction("DisplayEmployeeDepartments", "Report",
                        new
                        {
                            departmentID = empReportVM.DepartmentID,
                            option = sort
                        });
                case "department-salary":
                    sort = "department-salary";
                    return RedirectToAction("DisplayEmployeeDepartments", "Report",
                        new
                        {
                            departmentID = empReportVM.DepartmentID,
                            option = sort
                        });
            }

            empReportVM.Departments = report.GetDepartments(empReportVM.Keyword);
            return View(empReportVM);
        }

        [HttpGet]
        public IActionResult SearchPosition(string option)
        {
            EmployeeReportVM empReportVM = new EmployeeReportVM();
            empReportVM.Option = option;
            return View(empReportVM);
        }

        [HttpPost]
        public IActionResult SearchPosition(EmployeeReportVM empReportVM)
        {
            return RedirectToAction("DisplayPositions", "Report",
                new { option = empReportVM.Option, keyword = empReportVM.Keyword});
        }

        [HttpGet]
        public IActionResult DisplayPositions(string option, string keyword)
        {
            EmployeeReportVM empReportVM = new EmployeeReportVM();
            empReportVM.Option = option;
            empReportVM.Keyword = keyword;

            empReportVM.Positions = report.GetPositions(empReportVM.Keyword);
            return View(empReportVM);
        }

        [HttpPost]
        public IActionResult DisplayPositions(EmployeeReportVM empReportVM)
        {
            string sort = "";
            switch (empReportVM.Option)
            {
                case "position":
                    sort = "position-none";
                    return RedirectToAction("DisplayEmployeePositions", "Report",
                        new { positionID = empReportVM.PositionID,
                        option = sort});
                case "position-name-asc":
                    sort = "position-name-asc";
                    return RedirectToAction("DisplayEmployeePositions", "Report",
                        new
                        {
                            positionID = empReportVM.PositionID,
                            option = sort
                        });
                case "position-name-desc":
                    sort = "position-name-desc";
                    return RedirectToAction("DisplayEmployeePositions", "Report",
                        new
                        {
                            positionID = empReportVM.PositionID,
                            option = sort
                        });
                case "position-salary":
                    sort = "position-salary";
                    return RedirectToAction("DisplayEmployeePositions", "Report",
                        new
                        {
                            positionID = empReportVM.PositionID,
                            option = sort
                        });
            }

            empReportVM.Positions = report.GetPositions(empReportVM.Keyword);
            return View(empReportVM);
        }

        [HttpGet]
        public IActionResult DisplayEmployeePositions(int positionID, string option)
        {
            EmployeeReportVM empReportVM = new EmployeeReportVM();
            empReportVM.PositionID = positionID;
            empReportVM.PositionName = report.GetPositionName(empReportVM.PositionID);
            empReportVM.Employees = report.GetEmployees(empReportVM.PositionID, option);
            return View(empReportVM);
        }

        [HttpGet]
        public IActionResult DisplayEmployeeDepartments(int departmentID, string option)
        {
            EmployeeReportVM empReportVM = new EmployeeReportVM();
            empReportVM.DepartmentID = departmentID;
            empReportVM.DepartmentName = report.GetDepartmentName(empReportVM.DepartmentID);
            empReportVM.Employees = report.GetEmployees(empReportVM.DepartmentID, option);
            return View(empReportVM);
        }
    }
}
