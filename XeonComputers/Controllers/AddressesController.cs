using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Address;

namespace XeonComputers.Controllers
{
    public class AddressesController : BaseController
    {
        private readonly IAdressesService addressesService;

        public AddressesController(IAdressesService addressesService)
        {
            this.addressesService = addressesService;
        }

        public IActionResult Add(AddAddressesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Orders");
            }

            var address = this.addressesService.CreateAddress(model.Street, model.Description, model.CityName, model.CityPostcode);
            this.addressesService.AddAddressToUser(this.User.Identity.Name, address);

            return this.RedirectToAction("Create", "Orders");
        }
    }
}