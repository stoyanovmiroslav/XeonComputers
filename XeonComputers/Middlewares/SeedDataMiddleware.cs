using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Middlewares
{
    public class SeedDataMiddleware
    {
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
            if (!await roleManager.RoleExistsAsync(Role.Admin.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            }

            if (!await roleManager.RoleExistsAsync(Role.Partner.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Partner.ToString()));
            }
        }

        private static async Task SeedUserInRoles(UserManager<XeonUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new XeonUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "AdminFirstName",
                    LastName = "AdminLastName",
                    ShoppingCart = new ShoppingCart()
                };

                var password = "123456";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Role.Admin.ToString());
                }
            }
        }
    }
}