using Microsoft.AspNetCore.Authentication.Cookies;

namespace EmployeeManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services for authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login"; // Redirect to login page if unauthorized
                    options.LogoutPath = "/Login/Logout"; // Redirect here after logout
                    options.AccessDeniedPath = "/AccessDenied"; // Redirect if access denied
                });

            builder.Services.AddAuthorization();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                await next();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=login}/{id?}");

            app.Run();
        }
    }
}
