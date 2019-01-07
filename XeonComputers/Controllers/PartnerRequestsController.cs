using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Home;

namespace XeonComputers.Controllers
{
    [Authorize]
    public class PartnerRequestsController : BaseController
    {
        private const string YOUR_REQUEST_WAS_ACCEPTED = "Благодарим ви! Вашата заявка беше приета успешно!";

        public readonly IUsersService userService;
        public readonly IAdressesService adressesService;
        public readonly IPartnerRequestsService partnerRequestService;
        public readonly IMapper mapper;

        public PartnerRequestsController(IUsersService userService,
                                   IAdressesService adressesService,
                                   IPartnerRequestsService partnerRequestService,
                                   IMapper mapper)
        {
            this.userService = userService;
            this.adressesService = adressesService;
            this.partnerRequestService = partnerRequestService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            Company company = mapper.Map<Company>(model);
            Address address = this.adressesService.CreateAddress(model.AddressStreet, null, model.AddressCityName, null);

            company.Address = address;
            this.userService.CreateCompany(company, this.User.Identity.Name);

            this.partnerRequestService.Create(this.User.Identity.Name);

            this.TempData["info"] = YOUR_REQUEST_WAS_ACCEPTED;

            return RedirectToAction("Index", "Home");
        }
    }
}