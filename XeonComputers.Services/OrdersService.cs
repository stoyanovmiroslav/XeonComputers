using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;
using XeonComputers.Enums;


namespace XeonComputers.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IUsersService userService;
        private readonly IShoppingCartsService shoppingCartService;
        private readonly XeonDbContext db;

        public OrdersService(IUsersService userService, IShoppingCartsService shoppingCartService, XeonDbContext db)
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
                    Quantity = shoppingCartProduct.Quantity,
                    Price = (isPartnerOrAdmin ? shoppingCartProduct.Product.ParnersPrice : shoppingCartProduct.Product.Price)
                };

                orderProducts.Add(orderProduct);
            }

            this.shoppingCartService.DeleteAllProductFromShoppingCart(username);

            order.OrderDate = DateTime.UtcNow.AddHours(GlobalConstants.BULGARIAN_HOURS_FROM_UTC_TIME);
            order.Status = Enums.OrderStatus.Unprocessed;
            order.PaymentStatus = Enums.PaymentStatus.Unpaid;
            order.OrderProducts = orderProducts;
            order.TotalPrice = order.OrderProducts.Sum(x => x.Quantity * x.Price);
            order.InvoiceNumber = order.Id.ToString().PadLeft(10, '0');

            this.db.SaveChanges();
        }

        public Order CreateOrder(string username)
        {
            var user = this.userService.GetUserByUsername(username);

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
            return this.db.Orders.Include(x => x.DeliveryAddress)
                                 .ThenInclude(x => x.City)
                                 .Include(x => x.XeonUser)
                                 .ThenInclude(x => x.Company)
                                 .FirstOrDefault(x => x.Id == orderId);
        }

        public IEnumerable<Order> GetUnprocessedOrders()
        {
            var orders = this.db.Orders.Include(x => x.DeliveryAddress)
                                       .ThenInclude(x => x.City)
                                       .Include(x => x.OrderProducts)
                                       .Where(x => x.Status == Enums.OrderStatus.Unprocessed);

            return orders;
        }

        public Order GetProcessingOrder(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            var order = this.db.Orders.Include(x => x.DeliveryAddress)
                                      .ThenInclude(x => x.City)
                                      .Include(x => x.OrderProducts)
                                      .FirstOrDefault(x => x.XeonUser.UserName == username && x.Status == Enums.OrderStatus.Processing);

            return order;
        }

        public IEnumerable<Order> GetProcessedOrders()
        {
            var orders = this.db.Orders.Include(x => x.DeliveryAddress)
                                       .ThenInclude(x => x.City)
                                       .Include(x => x.OrderProducts)
                                       .Where(x => x.Status == Enums.OrderStatus.Processed);

            return orders;
        }

        public IEnumerable<Order> GetUserOrders(string username)
        {
            var order = this.db.Orders.Where(x => x.XeonUser.UserName == username && x.Status != OrderStatus.Processing).ToList();

            return order;
        }

        public void SetOrderDetails(Order order, string fullName, string phoneNumber, PaymentType paymentType, int deliveryAddressId, decimal deliveryPrice)
        {
            order.Recipient = fullName;
            order.RecipientPhoneNumber = phoneNumber;
            order.PaymentType = paymentType;
            order.DeliveryAddressId = deliveryAddressId;
            order.DeliveryPrice = deliveryPrice;

            this.db.Update(order);
            this.db.SaveChanges();
        }

        public void ProcessOrder(int id)
        {
            var order = this.db.Orders.FirstOrDefault(x => x.Id == id && 
                                        (x.Status == OrderStatus.Unprocessed || x.Status == OrderStatus.Delivered));

            if (order == null)
            {
                return;
            }

            order.Status = Enums.OrderStatus.Processed;
            order.DispatchDate = DateTime.UtcNow.AddHours(GlobalConstants.BULGARIAN_HOURS_FROM_UTC_TIME);
            this.db.SaveChanges();
        }

        public void DeliverOrder(int id)
        {
            var order = this.db.Orders.FirstOrDefault(x => x.Id == id
                                            && x.Status == Enums.OrderStatus.Processed);

            if (order == null)
            {
                return;
            }

            order.Status = Enums.OrderStatus.Delivered;
            order.DeliveryDate = DateTime.UtcNow.AddHours(GlobalConstants.BULGARIAN_HOURS_FROM_UTC_TIME);
            this.db.SaveChanges();
        }

        public IEnumerable<OrderProduct> OrderProductsByOrderId(int id)
        {
            return this.db.OrderProducts.Include(x => x.Product)
                                        .ThenInclude(x => x.Images)
                                        .Where(x => x.OrderId == id).ToList();
        }

        public Order GetUserOrderById(int orderId, string username)
        {
            return this.db.Orders.Include(x => x.DeliveryAddress)
                              .ThenInclude(x => x.City)
                              .Include(x => x.XeonUser)
                              .ThenInclude(x => x.Company)
                              .FirstOrDefault(x => x.Id == orderId && x.XeonUser.UserName == username);
        }

        public IEnumerable<Order> GetDeliveredOrders()
        {
            var orders = this.db.Orders.Include(x => x.DeliveryAddress)
                                      .ThenInclude(x => x.City)
                                      .Include(x => x.OrderProducts)
                                      .Where(x => x.Status == Enums.OrderStatus.Delivered);

            return orders;
        }

        public void SetEasyPayNumber(Order order, string easyPayNumber)
        {
            order.EasyPayNumber = easyPayNumber;
            this.db.SaveChanges();
        }

        public bool SetOrderStatusByInvoice(string invoiceNumber, string status)
        {
            var isOrderStatus = Enum.TryParse(typeof(PaymentStatus), status, true, out object paymentStatus);
            var order = this.db.Orders.FirstOrDefault(x => x.InvoiceNumber == invoiceNumber);

            if (order == null || !isOrderStatus)
            {
                return false;
            }

            order.PaymentStatus = (PaymentStatus)paymentStatus;
            this.db.SaveChanges();
            return true;
        }
    }
}