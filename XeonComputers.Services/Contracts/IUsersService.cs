using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IUsersService
    {
        XeonUser GetUserByUsername(string username);

        bool CreateCompany(Company company, string username);

        IEnumerable<XeonUser> GetUsersWithPartnersRequsts();

        void AddUserToRole(string username, string role);

        void RemoveUserFromToRole(string name, string role);

        IEnumerable<XeonUser> GetUsersByRole(string role);
    }
}