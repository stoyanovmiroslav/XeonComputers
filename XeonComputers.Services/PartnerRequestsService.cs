using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class PartnerRequestsService : IPartnerRequestsService
    {
        private readonly IUsersService userService;
        private readonly XeonDbContext db;

        public PartnerRequestsService(IUsersService userService, XeonDbContext db)
        {
            this.userService = userService;
            this.db = db;
        }

        public void Create(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null || this.db.PartnerRequests.Any(x => x.XeonUser == user))
            {
                return;
            }

            var partnerRequest = new PartnerRequest { XeonUser = user };

            this.db.PartnerRequests.Add(partnerRequest);
            this.db.SaveChanges();
        }

        public IEnumerable<PartnerRequest> GetPartnetsRequests()
        {
            return this.db.PartnerRequests.ToList();
        }

        public void Remove(int id)
        {
            var partnerRequest = this.db.PartnerRequests.FirstOrDefault(x => x.Id == id);
            if (partnerRequest == null)
            {
                return;
            }

            this.db.PartnerRequests.Remove(partnerRequest);
            this.db.SaveChanges();
        }
    }
}