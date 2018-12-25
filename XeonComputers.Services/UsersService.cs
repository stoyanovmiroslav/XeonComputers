using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<XeonUser> userManager;
        private readonly XeonDbContext db;

        public UsersService(XeonDbContext db, 
                            UserManager<XeonUser> userManager)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public XeonUser GetUserByUsername(string username)
        {
            return this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();
        }

        public bool CreateCompany(Company company, string username)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            user.Company = company;
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<XeonUser> GetUsersWithPartnersRequsts()
        {
            return this.db.Users.Include(x => x.Company)
                                .ThenInclude(x => x.Address)
                                .ThenInclude(x => x.City)
                                .Include(x => x.PartnerRequest)
                                .Where(x => x.PartnerRequest != null)
                                .ToList();
        }

        public void AddUserToRole(string username, string role)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return;
            }

            this.userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
        }

        public void RemoveUserFromToRole(string username, string role)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return;
            }

            this.userManager.RemoveFromRoleAsync(user, role).GetAwaiter().GetResult();
        }

        public IEnumerable<XeonUser> GetUsersByRole(string role)
        {
            var usersOfRole = this.userManager.GetUsersInRoleAsync(role).GetAwaiter().GetResult();

            return this.db.Users.Include(x => x.Company)
                                .ThenInclude(x => x.Address)
                                .ThenInclude(x => x.City)
                                .Where(x => usersOfRole.Any(u => u.Id == x.Id))
                                .ToList();
        }
    }
}