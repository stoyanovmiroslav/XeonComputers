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
using X.PagedList;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ProductsController : AdministratorController
    {
        private const int DEFAULT_PAGE_SIZE = 8;
        private const int DEFAULT_PAGE_NUMBER = 1;

        private readonly IProductsService productService;
        private readonly IChildCategoriesService childCategoriesService;
        private readonly IImagesService imageService;
        private readonly IMapper mapper;

        public ProductsController(IProductsService productService,
                                  IChildCategoriesService childCategoriesService,
                                  IImagesService imageService, 
                                  IMapper mapper)
        {
            this.productService = productService;
            this.childCategoriesService = childCategoriesService;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        public IActionResult AllHide(int? pageNumber, int? pageSize)
        {
            //TODO
            var products = this.productService.GetHideProducts();

            pageNumber = pageNumber ?? DEFAULT_PAGE_NUMBER;
            pageSize = pageSize ?? DEFAULT_PAGE_SIZE;
            var pageProductsViewMode = products.ToPagedList(pageNumber.Value, pageSize.Value);
           
            return View(pageProductsViewMode);
        }

        public IActionResult All(int? pageNumber, int? pageSize)
        {    
            //TODO
            var products = this.productService.GetAllProducts().OrderByDescending(x => x.Id).ToList();

            pageNumber = pageNumber ?? DEFAULT_PAGE_NUMBER;
            pageSize = pageSize ?? DEFAULT_PAGE_SIZE;
            var pageProductsViewMode = products.ToPagedList(pageNumber.Value, pageSize.Value);
        
            return View(pageProductsViewMode);
        }

        public IActionResult Create()
        {
            var childCategories = this.childCategoriesService.GetChildCategories();

            var categories = childCategories.Select(x => new SelectListItem
                                                        {
                                                           Value = x.Id.ToString(),
                                                           Text = $"{x.Name} ({x.ParentCategory.Name})"
                                                        }).ToList();

            var model = new CreateProductViewModel { ChildCategories = categories };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var childCategories = this.childCategoriesService.GetChildCategories();

                model.ChildCategories = childCategories.Select(x => new SelectListItem
                                                        {
                                                            Value = x.Id.ToString(),
                                                            Text = $"{x.Name} ({x.ParentCategory.Name})"
                                                        }).ToList();

                return View(model);
            }

            var product = this.mapper.Map<Product>(model);

            this.productService.AddProduct(product);

            if (model.FormImages != null)
            {
                int existingImages = 0;
                var imageUrls = await this.imageService.UploadImages(model.FormImages.ToList(), existingImages,
                                                                GlobalConstants.PRODUCT_PATH_TEMPLATE, product.Id);

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

            var childCategories = this.childCategoriesService.GetChildCategories();

            var model = this.mapper.Map<EditProductViewModel>(product);

            model.ChildCategories = childCategories.Select(x => new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = $"{x.Name} ({x.ParentCategory.Name})"
                                    }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var childCategories = this.childCategoriesService.GetChildCategories();

                 model.ChildCategories = childCategories.Select(x => new SelectListItem
                 {
                     Value = x.Id.ToString(),
                     Text = $"{x.Name} ({x.ParentCategory.Name})"
                 }).ToList();


                return View(model);
            }

            var product = this.mapper.Map<Product>(model);

            this.productService.EditProduct(product);

            if (model.FormImages != null)
            {
                int existingImages = productService.GetImages(product.Id).Count();
                var imageUrls = await this.imageService.UploadImages(model.FormImages.ToList(), existingImages,
                                                                GlobalConstants.PRODUCT_PATH_TEMPLATE, product.Id);

                this.productService.AddImageUrls(product.Id, imageUrls);
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Hide(int id)
        {
            var product = this.productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var deleteViewModel = mapper.Map<DeleteProductViewModel>(product);

            return View(deleteViewModel);
        }

        [HttpPost]
        public IActionResult HideConfirmed(int id)
        {
            this.productService.HideProduct(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Show(int id)
        {
            var product = this.productService.GetHideProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var deleteViewModel = mapper.Map<DeleteProductViewModel>(product);

            return View(deleteViewModel);
        }

        [HttpPost]
        public IActionResult ShowConfirmed(int id)
        {
            this.productService.ShowProduct(id);

            return RedirectToAction(nameof(All));
        }
    }
}