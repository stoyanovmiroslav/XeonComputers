using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Services.Contracts
{
    public interface IOrdersService
    {
        Order CreateOrder(string username);

        Order GetProcessingOrder(string username);

        void CompleteProcessingOrder(string username, bool isPartnerOrAdmin);

        Order GetOrderById(int orderId);

        void SetOrderDetails(Order order, string fullName, string phoneNumber, PaymentType paymentType, int deliveryAddressId);

        IEnumerable<Order> GetUserOrders(string name);
    }
}