using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.Suppliers;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class SuppliersController : AdministratorController
    {
        private readonly ISuppliersService suppliersService;
        private readonly IMapper mapper;

        public SuppliersController(ISuppliersService suppliersService, IMapper mapper)
        {
            this.suppliersService = suppliersService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.suppliersService.Create(model.Name, model.PriceToHome, model.PriceToOffice);
    
            return this.RedirectToAction(nameof(All));
        }

        public IActionResult All()
        {
            var suppliers = this.suppliersService.All();

            var supplierViewModels = mapper.Map<IList<SupplierViewModel>>(suppliers);

            return View(supplierViewModels);
        }

        public IActionResult MakeDafault(int id)
        {
            this.suppliersService.MakeDafault(id);
          
            return this.RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            this.suppliersService.Delete(id);

            return this.RedirectToAction(nameof(All));
        }

        public IActionResult Edit(int id)
        {
            Supplier supplier = this.suppliersService.GetSupplierById(id);

            var editViewModel = mapper.Map<EditSupplierViewModel>(supplier);

            return this.View(editViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.suppliersService.Edit(model.Id, model.Name, model.PriceToHome, model.PriceToOffice);

            return this.RedirectToAction(nameof(All));
        }
    }
}