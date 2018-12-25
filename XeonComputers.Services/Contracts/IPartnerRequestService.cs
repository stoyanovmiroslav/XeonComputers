using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IPartnerRequestService
    {
        void Create(string name);

        void Remove(int id);

        IEnumerable<PartnerRequest> GetPartnetsRequests();
    }
}
