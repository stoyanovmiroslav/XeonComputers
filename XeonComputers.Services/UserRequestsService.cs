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
    public class UserRequestsService : IUserRequestsService
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
                RequestDate = DateTime.UtcNow.AddHours(GlobalConstants.BULGARIAN_HOURS_FROM_UTC_TIME)
            };

            this.db.UserRequests.Add(userRequest);
            this.db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var userRequest = this.GetRequestById(id);

            if (userRequest == null)
            {
                return false;
            }

            this.db.UserRequests.Remove(userRequest);
            this.db.SaveChanges();

            return true;
        }

        public UserRequest GetRequestById(int id)
        {
            return this.db.UserRequests.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<UserRequest> GetUnseenRequests()
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

        public void Unseen(int id)
        {
            var userRequest = this.GetRequestById(id);

            if (userRequest == null)
            {
                return;
            }

            userRequest.Seen = false;
            this.db.SaveChanges();
        }
    }
}