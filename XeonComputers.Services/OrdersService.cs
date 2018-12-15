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
        private readonly IShoppingCartService shoppingCartService;
        private readonly XeonDbContext db;

        public OrdersService(IUsersService userService, IShoppingCartService shoppingCartService, XeonDbContext db)
        {
            this.userService = userService;
            this.shoppingCartService = shoppingCartService;
            this.db = db;
        }

        public void CompleteProcessingOrder(string username, bool isPartnerOrAdmin)
        {
            var order = this.GetProcessingOrder(username);
            if (order == null)
            {
                return;
            }

            var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(username).ToList();
            if (shoppingCartProducts == null || shoppingCartProducts.Count == 0)
            {
                return;
            }

            List<OrderProduct> orderProducts = new List<OrderProduct>();

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Order = order,
                    Product = shoppingCartProduct.Product,
                    Quantity = shoppingCartProduct.Quantity
                };

                orderProducts.Add(orderProduct);
            }

            this.shoppingCartService.DeleteAllProductFromsShoppingCart(username);
            
            order.OrderDate = DateTime.UtcNow;
            order.Status = Enums.OrderStatus.Processed;
            order.PaymentStatus = Enums.PaymentStatus.Unpaid;
            order.OrderProducts = orderProducts;
            order.TotalPrice = order.OrderProducts.Sum(x => x.Quantity * (isPartnerOrAdmin ? x.Product.ParnersPrice : x.Product.Price));

            this.db.SaveChanges();
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

        public Order GetOrderById(int orderId)
        {
            return this.db.Orders.FirstOrDefault(x => x.Id == orderId);
        }

        public Order GetProcessingOrder(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            var order = this.db.Orders.Include(x => x.DeliveryAddress).ThenInclude(x => x.City)
                                      .Include(x => x.OrderProducts) 
                               .FirstOrDefault(x => x.XeonUser.UserName == username && x.Status == Enums.OrderStatus.Processing);

            return order;
        }

        public IEnumerable<Order> GetUserOrders(string username)
        {
            var order = this.db.Orders.Where(x => x.XeonUser.UserName == username).ToList();

            return order;
        }

        public void SetOrderDetails(Order order, string fullName, string phoneNumber, PaymentType paymentType, int deliveryAddressId)
        {
            order.Recipient = fullName;
            order.RecipientPhoneNumber = phoneNumber;
            order.PaymentType = paymentType;
            order.DeliveryAddressId = deliveryAddressId;

            this.db.Update(order);
            this.db.SaveChanges();
        }
    }
}