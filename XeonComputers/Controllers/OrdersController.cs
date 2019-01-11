using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.Suppliers;
using XeonComputers.Common;
using XeonComputers.Enums;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services;
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

        private readonly IAdressesService adressesService;
        private readonly IUsersService usersService;
        private readonly IOrdersService orderService;
        private readonly IShoppingCartsService shoppingCartService;
        private readonly ISuppliersService suppliersService;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly IViewRender viewRender;

        public OrdersController(IAdressesService adressesService, IUsersService usersService,
                                IOrdersService orderService, IShoppingCartsService shoppingCartService,
                                ISuppliersService suppliersService, IMapper mapper, IEmailSender emailSender, IViewRender viewRender)
        {
            this.usersService = usersService;
            this.adressesService = adressesService;
            this.orderService = orderService;
            this.shoppingCartService = shoppingCartService;
            this.suppliersService = suppliersService;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.viewRender = viewRender;
        }

        public IActionResult Create()
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            var addresses = this.adressesService.GetAllUserAddresses(this.User.Identity.Name);
            var addressesViewModel = mapper.Map<IList<OrderAdressViewModel>>(addresses);

            var user = this.usersService.GetUserByUsername(this.User.Identity.Name);
            var fullName = $"{user.FirstName} {user.LastName}";

            var suppliers = this.suppliersService.All();
            var supplierViewModels = mapper.Map<IList<SupplierViewModel>>(suppliers);

            var createOrderViewModel = new CreateOrderViewModel
            {
                OrderAddressesViewModel = addressesViewModel.ToList(),
                FullName = fullName,
                PhoneNumber = user.PhoneNumber,
                SuppliersViewModel = supplierViewModels
            };

            return this.View(createOrderViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderViewModel model)
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
                order = this.orderService.CreateOrder(this.User.Identity.Name);
            }

            decimal deliveryPrice = suppliersService.GetDiliveryPrice(model.SupplierId, model.DeliveryType);
            this.orderService.SetOrderDetails(order, model.FullName, model.PhoneNumber, model.PaymentType, model.DeliveryAddressId.Value, deliveryPrice);

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

            if (order.PaymentType == PaymentType.EasyPay || order.PaymentType == PaymentType.CashОnDelivery)
            {
                var message = this.viewRender.Render("EmailTemplates/ConfirmOrder", order);
                await this.emailSender.SendEmailAsync(order.XeonUser.Email, string.Format(REGISTERED_ORDER, order.Id), message);

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