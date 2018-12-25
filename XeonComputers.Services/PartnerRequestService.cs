using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class PartnerRequestService : IPartnerRequestService
    {
        private readonly IUsersService userService;
        private readonly XeonDbContext db;

        public PartnerRequestService(IUsersService userService, XeonDbContext db)
        {
            this.userService = userService;
            this.db = db;
        }

        public void Create(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
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