using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class UserRequestService : IUserRequestService
    {
        private readonly XeonDbContext db;

        public UserRequestService(XeonDbContext db)
        {
            this.db = db;
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
    }
}