using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.Components;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Components
{
    public class AdminNavbarComponent : ViewComponent
    {
        public readonly IPartnerRequestsService partnerRequestService;
        public readonly IUserRequestsService userRequestService;
        public readonly IOrdersService ordersService;

        public AdminNavbarComponent(IPartnerRequestsService partnerRequestService, 
                                    IUserRequestsService userRequestService,
                                    IOrdersService ordersService)
        {
            this.partnerRequestService = partnerRequestService;
            this.userRequestService = userRequestService;
            this.ordersService = ordersService;
        }

        public IViewComponentResult Invoke()
        {
            var partnerRequestsCount = this.partnerRequestService.GetPartnetsRequests().Count();
            var userRequestsCount = this.userRequestService.GetUnseenRequests().Count();
            var unprocessedOrdersCount = this.ordersService.GetUnprocessedOrders().Count();

            var viewModel = new AdminNavbarViewModel
            {
                PartnerRequestsCount = partnerRequestsCount,
                UserRequestsCount = userRequestsCount,
                UnprocessedOrdersCount = unprocessedOrdersCount
            };

            return this.View(viewModel);
        }
    }
}