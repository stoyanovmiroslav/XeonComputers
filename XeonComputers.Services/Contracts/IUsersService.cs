using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IUsersService
    {
        string GetCurrentUserUsername();

        XeonUser GetUserByUsername(string username);
    }
}