using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Areas.Administrator.Services.Contracts;
using XeonComputers.Models;
using XeonComputers.ViewModels;

namespace XeonComputers.Controllers
{
    public class HomeController : Controller
    {
        private IChildCategoryService childCategoryService;
        private IParentCategoryService parentCategoryService;
        private IProductService productService;

        public HomeController(IChildCategoryService childCategoryService, 
                              IParentCategoryService parentCategoryService,
                              IProductService productService)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            var categories = this.parentCategoryService.GetParentCategories();

            var products = this.productService.GetProducts();

            return View(categories);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
