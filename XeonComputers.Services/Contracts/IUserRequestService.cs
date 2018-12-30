using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Services.Contracts
{
    public interface IUserRequestService
    {
        void Create(string title, string email, string content);
    }
}
