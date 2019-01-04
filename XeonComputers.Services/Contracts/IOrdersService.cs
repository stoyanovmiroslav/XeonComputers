﻿using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Services.Contracts
{
    public interface IOrdersService
    {
        Order CreateOrder(string username, decimal deliveryPrice);

        Order GetProcessingOrder(string username);

        void CompleteProcessingOrder(string username, bool isPartnerOrAdmin);

        Order GetOrderById(int orderId);

        void SetOrderDetails(Order order, string fullName, string phoneNumber, PaymentType paymentType, int deliveryAddressId);

        void ProcessOrder(int id);

        IEnumerable<Order> GetUserOrders(string name);

        IEnumerable<Order> GetUnprocessedOrders();

        IEnumerable<Order> GetProcessedOrders();

        void DeliverOrder(int id);

        IEnumerable<OrderProduct> OrderProductsByOrderId(int id);

        Order GetUserOrderById(int orderId, string username);

        IEnumerable<Order> GetDeliveredOrders();
    }
}