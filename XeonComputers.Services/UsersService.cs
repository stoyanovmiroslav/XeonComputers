using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<XeonUser> userManager;
        private readonly XeonDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UsersService(XeonDbContext db, UserManager<XeonUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.db = db;
        }

        public XeonUser GetUserByUsername(string username)
        {
            return this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();
        }

        public string GetCurrentUserUsername()
        {
            return this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        }
    }
}