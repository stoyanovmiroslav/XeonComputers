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

        public HomeController(IChildCategoryService childCategoryService, IParentCategoryService parentCategoryService)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
        }

        public IActionResult Index()
        {
            var categories = this.parentCategoryService.GetParentCategories();

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
