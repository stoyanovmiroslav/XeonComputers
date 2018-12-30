using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IUserRequestService
    {
        void Create(string title, string email, string content);

        IEnumerable<UserRequest> All();

        UserRequest GetRequestById(int id);

        void Seen(int id);

        IEnumerable<UserRequest> GetUnSeenRequests();
    }
}
