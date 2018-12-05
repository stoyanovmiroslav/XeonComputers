using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Services.Contracts
{
    public interface IOrdersService
    {
        Order CreateOrder(string name);

        Order GetProcessingOrder(string name);

        void SetAddress(Order order, int deliveryAddressId);

        void SetRecipientDetails(Order order, string fullName, string phoneNumber, PaymentType paymentType);
    }
}