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
using XeonComputers.Common;

namespace XeonComputers.Controllers
{
    public class HomeController : BaseController
    {
        private const int DEFAULT_PAGE_SIZE = 8;
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const string YOUR_REQUEST_WAS_ACCEPTED = "Благодарим ви! Вашето запитване беше приета успешно!";
        private const string NO_RESULTS_FOUND = "Няма намерени резултати";

        private readonly IChildCategoriesService childCategoryService;
        private readonly IParentCategoriesService parentCategoryService;
        private readonly IProductsService productService;
        private readonly IUserRequestsService userRequestService;
        private readonly IMapper mapper;

        public HomeController(IChildCategoriesService childCategoryService,
                              IParentCategoriesService parentCategoryService,
                              IProductsService productService,
                              IUserRequestsService userRequestService,
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

        public IActionResult GetProduct(string term)
        {
            var products = this.productService.GetProductsBySearch(term);

            if (products.Count() == 0)
            {
                return Json(new List<SearchViewModel>
                {
                    new SearchViewModel {Value = NO_RESULTS_FOUND, Url = string.Empty }
                });
            }

            var result = products.Select(x => new SearchViewModel
            {
                Value = x.Name,
                Url = string.Format(GlobalConstants.URL_TEMPLATE_AUTOCOMPLETE, x.Id)
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

            this.TempData["info"] = YOUR_REQUEST_WAS_ACCEPTED;

            this.userRequestService.Create(model.Title, model.Email, model.Content);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Payments()
        {
            return View();
        }

        public IActionResult Delivery()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
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