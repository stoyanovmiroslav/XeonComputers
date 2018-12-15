using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using XeonComputers.Services.Contracts;
using XeonComputers.Models;
using XeonComputers.ViewModels;
using XeonComputers.ViewModels.Home;
using XeonComputers.Models.Enums;

namespace XeonComputers.Controllers
{
    public class HomeController : BaseController
    {
        private const int DEFAULT_PAGE_SIZE = 8;
        private const int DEFAULT_PAGE_NUMBER = 1;

        private IChildCategoriesService childCategoryService;
        private IParentCategoriesService parentCategoryService;
        private IProductsService productService;

        public HomeController(IChildCategoriesService childCategoryService,
                              IParentCategoriesService parentCategoryService,
                              IProductsService productService)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
            this.productService = productService;
        }

        public IActionResult Index(IndexViewModel model)
        {
            var categories = this.parentCategoryService.GetParentCategories();
            var categoriesViewModel = categories.Select(x => new IndexParentCategoriesViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ChildCategories = x.ChildCategories.Select(c => new IndexChildCategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            }).ToList();

            var products = this.productService.GetProductsFilter(model.SearchString, model.ChildCategoryId);
            products = this.productService.OrderBy(products, model.SortBy).ToList();

            var productsViewModel = products.Select(x => new IndexProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParnersPrice = x.ParnersPrice,
                Price = x.Price,
                ImageUrl = x.Images.FirstOrDefault()?.ImageUrl
            }).ToList();

            int pageNumber = model.PageNumber ?? DEFAULT_PAGE_NUMBER;
            int pageSize = model.PageSize ?? DEFAULT_PAGE_SIZE;
            var pageProductsViewMode = productsViewModel.ToPagedList(pageNumber, pageSize);

            model.ProductsViewModel = pageProductsViewMode;
            model.CategoriesViewModel = categoriesViewModel;
 
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult GetProduct(string term)
        {
            var products = this.productService.GetProductsBySearch(term);

            //TODO: Url Pass
            var result = products.Select(x => new
            {
                value = x.Name,
                url = $"https://localhost:44374/Products/Details/{x.Id}"
            });

            return Json(result);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}