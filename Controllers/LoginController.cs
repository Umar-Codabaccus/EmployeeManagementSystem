using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EmployeeManagementSystem.Controllers {
    public class LoginController : Controller {
        private LoginDAL loginDAL;

        public LoginController() {
            loginDAL = new LoginDAL();
        }

        [HttpGet]
        public IActionResult login() {
            ViewBag.Error = false;
            ViewBag.Loginfailed = false;
            return View(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> login(Login login) {
            ViewBag.Error = false;
            ViewBag.Loginfailed = false;

            login = loginDAL.RetrieveLoginDetails(login.Username, login.Password);


            if (login.LoginSuccessfull) {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, login.RoleName),
                    new Claim("EmployeeID", login.EmployeeID.ToString())
                };

                // Create claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties {
                        IsPersistent = true // Remember Login
                    });

                return RedirectToAction("Index", login.RoleName);
            }

            // Check if login is not successfull when a username and a password have been entered.
            if (!login.LoginSuccessfull && login.UsernameEntered) {
                ViewBag.LoginFailed = true;
            }

            // Check if username or password has been entered
            if (!login.LoginSuccessfull && !login.UsernameEntered) {
                ViewBag.Error = true;
            }

            return View(login);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            // Sign out the user and clear the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page
            return RedirectToAction("Login");
        }
    }
}
