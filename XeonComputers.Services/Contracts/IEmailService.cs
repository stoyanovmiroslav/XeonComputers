using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IEmailService
    {
        Task SentConfirmationOrderEmail(Order order);
    }
}
