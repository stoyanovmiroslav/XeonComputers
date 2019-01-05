using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.ViewModels.Orders;
using XeonComputers.Common;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Payments;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Components
{
    public class OrderProductsComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;
        private readonly IMapper mapper;

        public OrderProductsComponent(IOrdersService ordersService, IMapper mapper)
        {
            this.ordersService = ordersService;
            this.mapper = mapper;
        }

        public IViewComponentResult Invoke(int orderId)
        {
            var orderProducts = this.ordersService.OrderProductsByOrderId(orderId);

            var orderProductsViewModel = mapper.Map<IList<OrderProductsViewModel>>(orderProducts);

            return this.View(orderProductsViewModel);
        }
    }
}