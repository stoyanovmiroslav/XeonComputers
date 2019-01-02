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
        public readonly IPartnerRequestService partnerRequestService;
        public readonly IUserRequestService userRequestService;

        public AdminNavbarComponent(IPartnerRequestService partnerRequestService, IUserRequestService userRequestService)
        {
            this.partnerRequestService = partnerRequestService;
            this.userRequestService = userRequestService;
        }

        public IViewComponentResult Invoke()
        {
            var partnerRequestsCount = this.partnerRequestService.GetPartnetsRequests().Count();
            var userRequestsCount = this.userRequestService.GetUnseenRequests().Count();

            var viewModel = new AdminNavbarViewModel
            {
                PartnerRequestsCount = partnerRequestsCount,
                UserRequestsCount = userRequestsCount
            };

            return this.View(viewModel);
        }
    }
}