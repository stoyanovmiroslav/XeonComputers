using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Data;
using XeonComputers.Models;

namespace XeonComputers.Middlewares
{
    public class SeedDataMiddleware
    {
        private const string ROLE_ADMIN = "Admin";
        private const string ROLE_PARTNER = "Partner";

        private readonly RequestDelegate _next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<XeonUser> userManager,
                                      RoleManager<IdentityRole> roleManager, XeonDbContext db)
        {
            SeedRoles(roleManager).GetAwaiter().GetResult();

            SeedUserInRoles(userManager).GetAwaiter().GetResult();

            await _next(context);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(ROLE_ADMIN))
            {
                await roleManager.CreateAsync(new IdentityRole(ROLE_ADMIN));
            }

            if (!await roleManager.RoleExistsAsync(ROLE_PARTNER))
            {
                await roleManager.CreateAsync(new IdentityRole(ROLE_PARTNER));
            }
        }

        private static async Task SeedUserInRoles(UserManager<XeonUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new XeonUser
                {
                    UserName = "AdminUser",
                    Email = "admin@gmail.com",
                    FirstName = "AdminFirstName",
                    LastName = "AdminLastName"
                };

                var password = "123456";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ROLE_ADMIN);
                }
            }
        }
    }
}