using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IPartnerRequestsService
    {
        void Create(string name);

        void Remove(int id);

        IEnumerable<PartnerRequest> GetPartnetsRequests();
    }
}
