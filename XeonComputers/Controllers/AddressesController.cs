﻿using Microsoft.AspNetCore.Http;
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
            var address = this.addressesService.CreateAddress(model.DeliveryAddress, model.AdditionТoАddress, model.City, model.Postcode);

            this.addressesService.AddAddressesToUser(this.User.Identity.Name, address);

            return this.RedirectToAction("CreateAddress", "Orders");
        }
    }
}