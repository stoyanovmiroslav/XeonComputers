using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.PartnerRequests;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class PartnerRequestsController : AdministratorController
    {
        private readonly IUsersService userService;
        private readonly IAdressesService adressesService;
        private readonly IPartnerRequestsService partnerRequestService;
        private readonly IMapper mapper;

        public PartnerRequestsController(IUsersService userService, IAdressesService adressesService,
                                         IPartnerRequestsService partnerRequestService, IMapper mapper)
        {
            this.userService = userService;
            this.adressesService = adressesService;
            this.partnerRequestService = partnerRequestService;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var userPartnerRequests = this.userService.GetUsersWithPartnersRequsts();

            var userPartnerRequestsViewModel = mapper.Map<IList<RequestUserCompanyViewModel>>(userPartnerRequests);

            return View(userPartnerRequestsViewModel);
        }

        public IActionResult Approve(int id, string username)
        {
            this.userService.AddUserToRole(username, Role.Partner.ToString());
            this.partnerRequestService.Remove(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Remove(int id)
        {
            this.partnerRequestService.Remove(id);
            return RedirectToAction(nameof(All));
        }
    }
}