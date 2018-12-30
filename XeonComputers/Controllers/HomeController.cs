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
using AutoMapper;

namespace XeonComputers.Controllers
{
    public class HomeController : BaseController
    {
        private const int DEFAULT_PAGE_SIZE = 8;
        private const int DEFAULT_PAGE_NUMBER = 1;

        private readonly IChildCategoriesService childCategoryService;
        private readonly IParentCategoriesService parentCategoryService;
        private readonly IProductsService productService;
        private readonly IUserRequestService userRequestService;
        private readonly IMapper mapper;

        public HomeController(IChildCategoriesService childCategoryService,
                              IParentCategoriesService parentCategoryService,
                              IProductsService productService,
                              IUserRequestService userRequestService,
                              IMapper mapper)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
            this.productService = productService;
            this.userRequestService = userRequestService;
            this.mapper = mapper;
        }

        public IActionResult Index(IndexViewModel model)
        {
            var categories = this.parentCategoryService.GetParentCategories();
            var products = this.productService.GetProductsFilter(model.SearchString, model.ChildCategoryId);
            products = this.productService.OrderBy(products, model.SortBy).ToList();

            var categoriesViewModel = mapper.Map<IList<IndexParentCategoriesViewModel>>(categories);
            var productsViewModel = mapper.Map<IList<IndexProductViewModel>>(products);

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
            if (this.User.Identity.IsAuthenticated)
            {
                ContactUserRequestViewModel model = new ContactUserRequestViewModel();
                model.Email = this.User.Identity.Name;

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactUserRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.userRequestService.Create(model.Title, model.Email, model.Content);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Payments()
        {
            ViewData["Message"] = "Your application Payments page.";

            return View();
        }

        public IActionResult Delivery()
        {
            ViewData["Message"] = "Your application Delivery page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}