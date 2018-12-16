using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XeonComputers.Services.Contracts;
using XeonComputers.Areas.Administrator.ViewModels.Products;
using XeonComputers.Common;
using XeonComputers.Data;
using XeonComputers.Models;
using AutoMapper;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ProductsController : AdministratorController
    {
        private readonly IProductsService productService;
        private readonly IImagesService imageService;
        private readonly IMapper mapper;

        public ProductsController(IProductsService productService, 
                                  IImagesService imageService, 
                                  IMapper mapper)
        {
            this.productService = productService;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var products = this.productService.GetProducts();

            return View(products);
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
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var childCategories = this.productService.GetChildCategories();

                ViewData["ChildCategoryId"] = childCategories.Select(x => new SelectListItem
                                                             {
                                                                 Value = x.Id.ToString(),
                                                                 Text = x.Name
                                                             }).ToList();

                return View(model);
            }

            var product = this.mapper.Map<Product>(model);

            this.productService.AddProduct(product);

            if (model.FormImages != null)
            {
                int existingImages = 0;
                var imageUrls = await this.imageService.UploadImages(model.FormImages.ToList(), existingImages,
                                                                GlobalConstans.PRODUCT_PATH_TEMPLATE, product.Id);

                this.productService.AddImageUrls(product.Id, imageUrls);
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Edit(int id)
        {
            var product = this.productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var childCategories = this.productService.GetChildCategories();

            ViewData["ChildCategoryId"] = childCategories.Select(x => new SelectListItem
                                                         {
                                                             Value = x.Id.ToString(),
                                                             Text = x.Name
                                                         }).ToList();

            var model = this.mapper.Map<EditProductViewModel>(product);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var childCategories = this.productService.GetChildCategories();

                ViewData["ChildCategoryId"] = childCategories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

                return View(model);
            }

            var product = this.mapper.Map<Product>(model);

            this.productService.EditProduct(product);

            if (model.FormImages != null)
            {
                int existingImages = productService.GetImages(product.Id).Count();
                var imageUrls = await this.imageService.UploadImages(model.FormImages.ToList(), existingImages,
                                                                GlobalConstans.PRODUCT_PATH_TEMPLATE, product.Id);

                this.productService.AddImageUrls(product.Id, imageUrls);
            }

            return RedirectToAction(nameof(All));
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
        public IActionResult DeleteConfirmed(int id)
        {
            this.productService.RemoveProduct(id);

            return RedirectToAction(nameof(All));
        }
    }
}