using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IUsersService userService;
        private readonly XeonDbContext db;

        public OrdersService(IUsersService userService, XeonDbContext db)
        {
            this.userService = userService;
            this.db = db;
        }

        public Order CreateOrder(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            var processingOrder = this.GetProcessingOrder(username);
            if (processingOrder != null)
            {
                return processingOrder;
            }

            var order = new Order
            {
                Status = Enums.OrderStatus.Processing,
                XeonUser = user,
            };

            this.db.Orders.Add(order);
            this.db.SaveChanges();

            return order;
        }

        public Order GetProcessingOrder(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            var order = this.db.Orders.Include(x => x.DeliveryAddress).ThenInclude(x => x.City)
                               .FirstOrDefault(x => x.XeonUser.UserName == username || x.Status == Enums.OrderStatus.Processing);

            return order;
        }

        public void SetAddress(Order order, int deliveryAddressId)
        {
            order.DeliveryAddressId = deliveryAddressId;

            this.db.Update(order);
            this.db.SaveChanges();
        }

        public void SetRecipientDetails(Order order, string fullName, string phoneNumber, PaymentType paymentType)
        {
            order.Recipient = fullName;
            order.RecipientPhoneNumber = phoneNumber;
            order.PaymentType = paymentType;

            this.db.Update(order);
            this.db.SaveChanges();
        }
    }
}