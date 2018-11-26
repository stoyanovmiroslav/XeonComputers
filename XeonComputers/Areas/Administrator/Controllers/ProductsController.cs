using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XeonComputers.Areas.Administrator.Services.Contracts;
using XeonComputers.Areas.Administrator.ViewModels.Products;
using XeonComputers.Data;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ProductsController : AdministratorController
    {
        private IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult All()
        {
            var products = this.productService.GetProducts();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = this.productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new DetailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ChildCategoryName = product.ChildCategory.Name,
                Price = product.Price,
                ParnersPrice = product.Price,
                ProductType = product.ProductType,
                Specification = product.Specification
            };

            return View(productViewModel);
        }

        public IActionResult Create()
        {
            var childCategories = this.productService.GetChildCategories();

            var categories = childCategories.Select(x => new SelectListItem
                             {
                                Value = x.Id.ToString(),
                                Text = x.Name
                             }).ToList();

            var model = new CreateProductViewModel { ChildCategories = categories };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                this.productService.AddProduct(product);
                return RedirectToAction(nameof(All));
            }

            var childCategories = this.productService.GetChildCategories();

            ViewData["ChildCategoryId"] = new SelectList(childCategories, "Id", "Id", product.ChildCategoryId);
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = this.productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var childCategories = this.productService.GetChildCategories();

            ViewData["ChildCategoryId"] = new SelectList(childCategories, "Id", "Id", product.ChildCategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                this.productService.EditProduct(product);

                return RedirectToAction(nameof(All));
            }

            var childCategories = this.productService.GetChildCategories();

            ViewData["ChildCategoryId"] = new SelectList(childCategories, "Id", "Id", product.ChildCategoryId);
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = this.productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            this.productService.RemoveProduct(id);

            return RedirectToAction(nameof(All));
        }
    }
}