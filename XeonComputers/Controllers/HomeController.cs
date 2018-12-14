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

namespace XeonComputers.Controllers
{
    public class HomeController : BaseController
    {
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

        public IActionResult Index(int? page, int? childCategoryId, string searchString)
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

            //TODO: Implement IQueryable
            //TODO: OrderByPrice, OrderByRaiting
            IList<Product> products = null;
            
            if (searchString != null)
            {
                products = this.productService.GetProductsBySearch(searchString);
            }
            else if (childCategoryId != null)
            {
                products = this.productService.GetProductsByCategory(childCategoryId.Value).ToList();
            }
            else
            {
                products = this.productService.GetProductsQuery().ToList();
            }

            var productsViewModel = products.Select(x => new IndexProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParnersPrice = x.ParnersPrice,
                Price = x.Price,
                ImageUrl = x.Images.FirstOrDefault()?.ImageUrl
            }).ToList();

            var pageNumber = page ?? 1;
            int productsPerPage = 8;
            var onePageOfProducts = productsViewModel.ToPagedList(pageNumber, productsPerPage);

            var indexViewModel = new IndexViewModel
            {
                ProductsViewModel = onePageOfProducts,
                CategoriesViewModel = categoriesViewModel,
                ChildCategoryId = childCategoryId,
                SearchString = searchString
            };

            return View(indexViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult GetProduct(string term)
        {
            var products = this.productService.GetProductsBySearch(term);

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