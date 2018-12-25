using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.PartnerRequests;
using XeonComputers.Areas.Administrator.ViewModels.Partners;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class PartnersController : AdministratorController
    {
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        public PartnersController(IUsersService usersService, IMapper mapper)
        {
            this.usersService = usersService;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var allPartners = this.usersService.GetUsersByRole(Role.Partner.ToString());

            var allPartnersViewModel = mapper.Map<IList<AllPartnersViewModel>>(allPartners);

            return this.View(allPartnersViewModel);
        }

        public IActionResult Remove(string username)
        {
            this.usersService.RemoveUserFromToRole(username, Role.Partner.ToString());

            return RedirectToAction(nameof(All));
        }
    }
}
