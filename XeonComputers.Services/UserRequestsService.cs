using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class UserRequestsService : IUserRequestService
    {
        private readonly XeonDbContext db;

        public UserRequestsService(XeonDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<UserRequest> All()
        {
            return db.UserRequests;
        }

        public void Create(string title, string email, string content)
        {
            var userRequest = new UserRequest
            {
                Title = title,
                Email = email,
                Content = content,
                RequestDate = DateTime.UtcNow.AddHours(GlobalConstans.BULGARIAN_HOURS_FROM_UTC_TIME)
            };

            this.db.UserRequests.Add(userRequest);
            this.db.SaveChanges();
        }

        public UserRequest GetRequestById(int id)
        {
            return this.db.UserRequests.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<UserRequest> GetUnSeenRequests()
        {
            return this.db.UserRequests.Where(x => x.Seen == false);
        }

        public void Seen(int id)
        {
            var userRequest = this.GetRequestById(id);

            if (userRequest == null)
            {
                return;
            }

            userRequest.Seen = true;
            this.db.SaveChanges();
        }
    }
}