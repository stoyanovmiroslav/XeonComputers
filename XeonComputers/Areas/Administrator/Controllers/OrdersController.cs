using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.Orders;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class OrdersController : AdministratorController
    {
        private const string ERROR_MESSAGE_INVALID_ORDER_NUMBER = "Невалиден номер на поръчка, моля опитайте отново!";

        private readonly IOrdersService ordersService; 
        private readonly IMapper mapper;

        public OrdersController(IOrdersService ordersService, IMapper mapper)
        {
            this.ordersService = ordersService;
            this.mapper = mapper;
        }

        public IActionResult Process(int id)
        {
            this.ordersService.ProcessOrder(id);

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Deliver(int id)
        {
            this.ordersService.DeliverOrder(id);

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
        {
            var order = this.ordersService.GetOrderById(id);
            if (order == null)
            {
                this.TempData["error"] = ERROR_MESSAGE_INVALID_ORDER_NUMBER;
                return RedirectToAction("Index", "Home");
            }

            var orderProducts = this.ordersService.OrderProductsByOrderId(id);
            var orderProductsViewModel = mapper.Map<IList<OrderProductsViewModel>>(orderProducts);

            var orderViewModel = mapper.Map<OrderDetailsViewModel>(order);
            orderViewModel.OrderProductsViewModel = orderProductsViewModel;

            return this.View(orderViewModel);
        }

        public IActionResult Delivered()
        {
            var deliveredОrders = this.ordersService.GetDeliveredOrders();

            var deliveredОrdersViewModel = mapper.Map<IList<DeliveredОrdersViewModels>>(deliveredОrders);

            return View(deliveredОrdersViewModel);
        }
    }
}