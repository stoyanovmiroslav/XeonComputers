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

        bool AddUserToRole(string username, string role);

        bool RemoveUserFromToRole(string name, string role);

        IEnumerable<XeonUser> GetUsersByRole(string role);

        Company GetUserCompanyByUsername(string name);

        void EditFirstName(XeonUser user, string firstName);

        void EditLastName(XeonUser user, string lastName);
    }
}