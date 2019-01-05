using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Orders;

namespace XeonComputers.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private const string ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS = "За да продължите добавете продукти в кошницата!";
        private const string ERROR_MESSAGE_INVALID_ORDER_NUMBER = "Невалиден номер на поръчка, моля опитайте отново!";
        private const string YOUR_ORDER_WAS_SUCCESSFULLY_RECEIVED = "Вашата поръчка беше получена успешно!";
        private const string REGISTERED_ORDER = "Регистрирана поръчка #{0}";
        private const string PATH_CONFIRMATION_ORDER_EMAIL = "Views/EmailTemplates/ConfirmOrder.cshtml";

        private readonly IAdressesService adressesService;
        private readonly IUsersService usersService;
        private readonly IOrdersService orderService;
        private readonly IShoppingCartsService shoppingCartService;
        private readonly ISuppliersService suppliersService;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;

        public OrdersController(IAdressesService adressesService, IUsersService usersService,
                                IOrdersService orderService, IShoppingCartsService shoppingCartService,
                                ISuppliersService suppliersService, IMapper mapper, IEmailSender emailSender)
        {
            this.usersService = usersService;
            this.adressesService = adressesService;
            this.orderService = orderService;
            this.shoppingCartService = shoppingCartService;
            this.suppliersService = suppliersService;
            this.mapper = mapper;
            this.emailSender = emailSender;
        }

        public IActionResult Create()
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);
            if (order == null)
            {
                return this.RedirectToAction("Index", "ShoppingCart");
            }

            var addresses = this.adressesService.GetAllUserAddresses(this.User.Identity.Name);
            var addressesViewModel = mapper.Map<IList<OrderAdressViewModel>>(addresses);

            var user = this.usersService.GetUserByUsername(this.User.Identity.Name);
            var fullName = $"{user.FirstName} {user.LastName}";

            var createOrderViewModel = new CreateOrderViewModel
            {
                OrderAddressesViewModel = addressesViewModel.ToList(),
                FullName = fullName,
                PhoneNumber = user.PhoneNumber,
                DeliveryPrice = order.DeliveryPrice
            };

            return this.View(createOrderViewModel);
        }

        [HttpPost]
        public IActionResult Create(int supplierId, DeliveryType deliveryType)
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            decimal deliveryPrice = this.suppliersService.GetDiliveryPrice(supplierId, deliveryType);
            var order = this.orderService.CreateOrder(this.User.Identity.Name, deliveryPrice);

            var addresses = this.adressesService.GetAllUserAddresses(this.User.Identity.Name);
            var addressesViewModel = mapper.Map<IList<OrderAdressViewModel>>(addresses);

            var user = this.usersService.GetUserByUsername(this.User.Identity.Name);
            var fullName = $"{user.FirstName} {user.LastName}";

            var createOrderViewModel = new CreateOrderViewModel
            {
                OrderAddressesViewModel = addressesViewModel.ToList(),
                FullName = fullName,
                PhoneNumber = user.PhoneNumber,
                DeliveryPrice = order.DeliveryPrice
            };

            return this.View(createOrderViewModel);
        }

        [HttpPost]
        public IActionResult SetOrderDetails(CreateOrderViewModel model)
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);
            if (order == null)
            {
                return this.RedirectToAction("Index", "ShoppingCart");
            }

            this.orderService.SetOrderDetails(order, model.FullName, model.PhoneNumber, model.PaymentType, model.DeliveryAddressId.Value);

            return this.RedirectToAction(nameof(Complete));
        }

        public async Task<IActionResult> Complete()
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);
            var orderViewModel = mapper.Map<ConfirmOrderViewModel>(order);

            bool isPartnerOrAdmin = this.User.IsInRole(Role.Admin.ToString()) || this.User.IsInRole(Role.Partner.ToString());
            this.orderService.CompleteProcessingOrder(this.User.Identity.Name, isPartnerOrAdmin);

            if (order.PaymentType == PaymentType.CashОnDelivery || order.PaymentType == PaymentType.CashОnDelivery)
            {
                var email = this.usersService.GetUserByUsername(this.User.Identity.Name).Email;

                var emailMessageTempate = System.IO.File.ReadAllText(PATH_CONFIRMATION_ORDER_EMAIL);
                var message = string.Format(emailMessageTempate, orderViewModel.Recipient, orderViewModel.RecipientPhoneNumber, orderViewModel.DeliveryAddressCityName, orderViewModel.DeliveryAddressCityPostCode,
                                                                 orderViewModel.DeliveryAddressStreet, orderViewModel.DeliveryAddressDescription, orderViewModel.TotalPrice);

                var subject = string.Format(REGISTERED_ORDER, order.Id);

                await this.emailSender.SendEmailAsync(email, subject, message);

                this.TempData["info"] = YOUR_ORDER_WAS_SUCCESSFULLY_RECEIVED;
            }

            return this.View(orderViewModel);
        }

        public IActionResult My(int id)
        {
            IEnumerable<Order> orders = this.orderService.GetUserOrders(this.User.Identity.Name).OrderByDescending(x => x.Id);

            var myOrdersViewModel = mapper.Map<IList<MyOrderViewModel>>(orders);

            return this.View(myOrdersViewModel);
        }

        public IActionResult Details(int id)
        {
            var order = this.orderService.GetUserOrderById(id, this.User.Identity.Name);
            if (order == null)
            {
                this.TempData["error"] = ERROR_MESSAGE_INVALID_ORDER_NUMBER;
                return RedirectToAction(nameof(My));
            }

            var orderProducts = this.orderService.OrderProductsByOrderId(id);

            var orderViewModel = mapper.Map<OrderDetailsViewModel>(order);
            var orderProductsViewModel = mapper.Map<IList<OrderProductsViewModel>>(orderProducts);
            orderViewModel.OrderProductsViewModel = orderProductsViewModel;

            return this.View(orderViewModel);
        }
    }
}