using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Services.Contracts
{
    public interface IPaymentsService
    {
        string Encoded { get; set; }

        string EPay(decimal sum, string description);
    }
}