using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Enums;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class OrdersServiceTests
    {
        [Fact]
        public void CreateOrderShouldCreateOrder()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateOrder_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username };

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(username))
                        .Returns(user);

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);
            ordersService.CreateOrder(username);

            var orders = dbContext.Orders.ToList();

            Assert.Single(orders);
        }

        [Fact]
        public void GetProcessingOrderShouldReturnProcessingOrder()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetProcessingOrder_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Id = 1, Status = OrderStatus.Processing },
                    new Order { Id = 2, Status = OrderStatus.Delivered },
                    new Order { Id = 3, Status = OrderStatus.Processed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var order = ordersService.GetProcessingOrder(user.UserName);

            Assert.Equal(1, order.Id);
        }

        [Fact]
        public void GetOrderByIdShouldReturnOrder()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetOrderById_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var orderId = 3;
            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Id = 1, Status = OrderStatus.Processing },
                    new Order { Id = 2, Status = OrderStatus.Delivered },
                    new Order { Id = orderId, Status = OrderStatus.Processed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var order = ordersService.GetOrderById(orderId);

            Assert.Equal(orderId, order.Id);
            Assert.Equal(OrderStatus.Processed, order.Status);
        }

        [Fact]
        public void ProcessOrderShouldProcessOrder()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "ProcessOrder_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var orderId = 3;
            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Id = 1, Status = OrderStatus.Processing },
                    new Order { Id = 2, Status = OrderStatus.Delivered },
                    new Order { Id = orderId, Status = OrderStatus.Unprocessed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            ordersService.ProcessOrder(orderId);

            var order = dbContext.Orders.FirstOrDefault(x => x.Id == orderId);

            Assert.Equal(orderId, order.Id);
            Assert.Equal(OrderStatus.Processed, order.Status);
        }

        [Fact]
        public void GetUserOrdersShouldReturnAllUserOrders()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUserOrders_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var user = new XeonUser { UserName = "user@gmail.com" };
            var orders = new List<Order>
            {
                new Order { Status = OrderStatus.Processed, XeonUser = user },
                new Order { Status = OrderStatus.Delivered, XeonUser = user },
                new Order { Status = OrderStatus.Unprocessed, XeonUser = new XeonUser {UserName = "admin@gmail.com" } },
            };
            dbContext.Orders.AddRange(orders);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var userOrders = ordersService.GetUserOrders(user.UserName);

            Assert.Equal(2, userOrders.Count());
        }

        [Fact]
        public void GetUserOrderByIdShouldReturnOrder()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUserOrderById_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var orderId = 3;
            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Id = 1, Status = OrderStatus.Processing },
                    new Order { Id = 2, Status = OrderStatus.Delivered },
                    new Order { Id = orderId, Status = OrderStatus.Unprocessed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var order = ordersService.GetOrderById(orderId);

            Assert.Equal(orderId, order.Id);
        }

        [Fact]
        public void DeliverOrderShouldChangeOrderStatusToDeliver()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeliverOrder_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var orderId = 3;
            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Id = 1, Status = OrderStatus.Processing },
                    new Order { Id = 2, Status = OrderStatus.Delivered },
                    new Order { Id = orderId, Status = OrderStatus.Processed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            ordersService.DeliverOrder(orderId);

            var order = dbContext.Orders.FirstOrDefault(x => x.Id == orderId);

            Assert.Equal(OrderStatus.Delivered, order.Status);
        }

        [Fact]
        public void GetUnprocessedOrdersShouldReturnAllUnprocessedOrders()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUnprocessedOrders_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Status = OrderStatus.Unprocessed },
                    new Order { Status = OrderStatus.Delivered },
                    new Order { Status = OrderStatus.Unprocessed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var unprocessedOrders = ordersService.GetUnprocessedOrders();

            Assert.Equal(2, unprocessedOrders.Count());
        }

        [Fact]
        public void GetProcessedOrdersShouldReturnAllProcessedOrders()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetProcessedOrders_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>
                {
                    new Order { Status = OrderStatus.Processed },
                    new Order { Status = OrderStatus.Delivered },
                    new Order { Status = OrderStatus.Processed },
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var unprocessedOrders = ordersService.GetProcessedOrders();

            Assert.Equal(2, unprocessedOrders.Count());
        }

        [Fact]
        public void OrderProductsByOrderIdShouldReturnAllProductsWithSubmmitedOrderId()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderProductsByOrderId_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var order = new Order();
            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct{ Product = new Product { Name = "USB 2.0" }, Order = order },
                new OrderProduct{ Product = new Product { Name = "USB 1.0" }, Order = order },
                new OrderProduct{ Product = new Product { Name = "USB 3.0" }, Order = order },
                new OrderProduct{ Product = new Product { Name = "USB 4.0" }, Order = new Order() },
            };

            dbContext.OrderProducts.AddRange(orderProducts);
            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var orderProductsById = ordersService.OrderProductsByOrderId(order.Id);

            Assert.Equal(3, orderProductsById.Count());
        }

        [Fact]
        public void SetOrderDetailsShouldSetOrderDetails()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "SetOrderDetails_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var shoppingCartsService = new Mock<IShoppingCartsService>();

            var order = new Order { Status = OrderStatus.Processing };
            dbContext.Orders.Add(order);

            var address = new Address { Street = "str. Ivan Vazov" };
            dbContext.Addresses.Add(address);

            dbContext.SaveChanges();

            var usersService = new Mock<IUsersService>();
            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            var recipient = "Ivan Ivanov";
            var recipientPhoneNumber = "09823222112";
            var deliveryPrice = 4.50M;
            ordersService.SetOrderDetails(order, recipient, recipientPhoneNumber, PaymentType.CashОnDelivery, address.Id, deliveryPrice);

            Assert.Equal(recipient, order.Recipient);
            Assert.Equal(recipientPhoneNumber, order.RecipientPhoneNumber);
            Assert.Equal(PaymentType.CashОnDelivery, order.PaymentType);
            Assert.Equal(address.Id, order.DeliveryAddressId);
        }

        [Theory]
        [InlineData(false, 15, 1)]
        [InlineData(false, 30, 2)]
        [InlineData(false, 45, 3)]
        [InlineData(true, 25, 1)]
        [InlineData(true, 50, 2)]
        [InlineData(true, 75, 3)]
        public void CompleteProcessingOrderShouldCompleteProcessingOrder(bool isPartnerOrAdmin, decimal totalPrice, int quantity)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: $"CompleteProcessingOrder{totalPrice}_Orders_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Orders = new List<Order>{ new Order { Status = OrderStatus.Processing }},
                ShoppingCart = new ShoppingCart()
            };

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct
                {
                     Product = new Product { Name = "USB 1.0", Price = 10, ParnersPrice = 15 },
                     ShoppingCart = user.ShoppingCart,
                     Quantity = quantity
                },
                new ShoppingCartProduct
                {
                     Product = new Product { Name = "USB 2.0", Price = 5, ParnersPrice = 10 },
                     ShoppingCart = user.ShoppingCart,
                     Quantity = quantity
                }
            };

            dbContext.Users.Add(user);
            dbContext.ShoppingCartProducts.AddRange(shoppingCartProducts);
            dbContext.SaveChanges();

            var shoppingCartsService = new Mock<IShoppingCartsService>();
            shoppingCartsService.Setup(s => s.GetAllShoppingCartProducts(user.UserName))
                                .Returns(shoppingCartProducts);

            var usersService = new Mock<IUsersService>();
            usersService.Setup(u => u.GetUserByUsername(user.UserName))
                            .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName));

            var ordersService = new OrdersService(usersService.Object, shoppingCartsService.Object, dbContext);

            ordersService.CompleteProcessingOrder(user.UserName, isPartnerOrAdmin);

            var order = dbContext.Orders.FirstOrDefault(x => x.XeonUser.UserName == user.UserName);

            Assert.Equal(OrderStatus.Unprocessed, order.Status);
            Assert.Equal(2 , order.OrderProducts.Count());
            Assert.Equal(PaymentStatus.Unpaid, order.PaymentStatus);
            Assert.Equal(totalPrice, order.TotalPrice);
        }
    }                     
}