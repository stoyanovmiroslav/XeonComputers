using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Orders;

namespace XeonComputers.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
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
            var order = this.orderService.CreateOrder(this.User.Identity.Name);

            return this.RedirectToAction(nameof(CreateAddress));
        }

        public IActionResult CreateAddress()
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

            var createAddressViewModel = new CreateAddressOrderViewModel
            {
                OrderAddressesViewModel = addressesViewModel,
                OrderAdressViewModel = new OrderAdressViewModel()
            };

            return this.View(createAddressViewModel);
        }

        [HttpPost]
        public IActionResult CreateAddress(int deliveryAddressId)
        {
            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);

            this.orderService.SetAddress(order, deliveryAddressId);

            return this.RedirectToAction(nameof(CreateRecipient));
        }

        public IActionResult CreateRecipient()
        {
            var user = this.userService.GetUserByUsername(this.User.Identity.Name);

            var fullName = $"{user.FirstName} {user.LastName}";

            var recipientViewModel = new RecipientOrderViewModel
            {
                FullName = fullName,
                PhoneNumber = user.PhoneNumber
            };

            return this.View(recipientViewModel);
        }

        [HttpPost]
        public IActionResult CreateRecipient(string fullName, string phoneNumber, PaymentType paymentType)
        {
            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);

            if (order == null)
            {
                return this.RedirectToAction("Index", "ShoppingCart");
            }

            this.orderService.SetRecipientDetails(order, fullName, phoneNumber, paymentType);

            return this.RedirectToAction(nameof(Confirm));
        }

        public IActionResult Confirm()
        {
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

        public IActionResult Finish(int id)
        {
            // Fo=inish Order
            // Redirect to payment
            // Clear Shopping Cart

            var order = this.orderService.GetProcessingOrder(this.User.Identity.Name);
            var user = this.userService.GetUserByUsername(this.User.Identity.Name);
            var shoppingCart = this.shoppingCartService.GetAllShoppingCartProducts(this.User.Identity.Name);
           
            return this.View();
        }
    }
}