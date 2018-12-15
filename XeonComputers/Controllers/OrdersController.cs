using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        private readonly IAdressesService adressesService;
        private readonly IUsersService userService;
        private readonly IOrdersService orderService;
        private readonly IShoppingCartService shoppingCartService;

        public OrdersController(IAdressesService adressesService, IUsersService userService,
                                IOrdersService orderService, IShoppingCartService shoppingCartService)
        {
            this.userService = userService;
            this.adressesService = adressesService;
            this.orderService = orderService;
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Create()
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            var order = this.orderService.CreateOrder(this.User.Identity.Name);
            var addresses = this.adressesService.GetAllUserAddresses(this.User.Identity.Name);

            var addressesViewModel = addresses.Select(x => new OrderAdressViewModel
            {
                Id = x.Id,
                City = x.City.Name,
                Postcode = x.City.Postcode,
                DeliveryAddress = x.Street,
                AdditionТoАddress = x.Description
            }).ToList();

            var user = this.userService.GetUserByUsername(this.User.Identity.Name);
            var fullName = $"{user.FirstName} {user.LastName}";

            var createOrderViewModel = new CreateOrderViewModel
            {
                OrderAddressesViewModel = addressesViewModel,
                FullName = fullName,
                PhoneNumber = user.PhoneNumber
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
                var addresses = this.adressesService.GetAllUserAddresses(this.User.Identity.Name);
                var addressesViewModel = addresses.Select(x => new OrderAdressViewModel
                {
                    Id = x.Id,
                    City = x.City.Name,
                    Postcode = x.City.Postcode,
                    DeliveryAddress = x.Street,
                    AdditionТoАddress = x.Description
                }).ToList();

                model.OrderAddressesViewModel = addressesViewModel;
                return this.View(model);
            }

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);
            if (order == null)
            {
                return this.RedirectToAction("Index", "ShoppingCart");
            }
            
            this.orderService.SetOrderDetails(order, model.FullName, model.PhoneNumber, model.PaymentType, model.DeliveryAddressId.Value);

            return this.RedirectToAction(nameof(Confirm));
        }

        public IActionResult Confirm()
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);

            var orderViewModel = new ConfirmOrderViewModel
            {
                Recipient = order.Recipient,
                RecipientPhoneNumber = order.RecipientPhoneNumber,
                City = order.DeliveryAddress.City.Name,
                Street = order.DeliveryAddress.Street,
                Description = order.DeliveryAddress.Description,
                PostCode = order.DeliveryAddress.City.Postcode,
                PaymentType = order.PaymentType,
            };

            return this.View(orderViewModel);
        }

        public IActionResult Complete(int id)
        {
            if (!this.shoppingCartService.AnyProducts(this.User.Identity.Name))
            {
                this.TempData["error"] = ERROR_MESSAGE_TO_CONTINUE_ADD_PRODUCTS;
                return RedirectToAction("Index", "Home");
            }

            bool isPartnerOrAdmin = this.User.IsInRole(Role.Admin.ToString()) || this.User.IsInRole(Role.Partner.ToString());

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);
            this.orderService.CompleteProcessingOrder(this.User.Identity.Name, isPartnerOrAdmin);
           
            return this.RedirectToAction("Pay", "Payments", new { orderId = order.Id });
        }


        public IActionResult My(int id)
        {
            IEnumerable<Order> orders = this.orderService.GetUserOrders(this.User.Identity.Name);

            var myOrdersViewModel = orders.Select(x => new MyOrderViewModel
                                                  {
                                                     TotalPrice = x.TotalPrice,
                                                     PaymentStatus = x.PaymentStatus.GetDisplayName(),
                                                     PaymentType = x.PaymentType.GetDisplayName(),
                                                     Status = x.Status.GetDisplayName(),
                                                     Id = x.Id,
                                                  })
                                                  .OrderByDescending(x => x.Id)
                                                  .ToList();

            return this.View(myOrdersViewModel);
        }
    }
}