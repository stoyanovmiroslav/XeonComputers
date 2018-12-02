using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Services.Contracts;
using XeonComputers.Models;
using XeonComputers.ViewModels;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class HomeController : AdministratorController
    {
        private IChildCategoryService childCategoryService;
        private IParentCategoryService parentCategoryService;

        public HomeController(IChildCategoryService childCategoryService,
                                         IParentCategoryService parentCategoryService)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
        }

        public IActionResult Index()
        {
            var categories = this.parentCategoryService.GetParentCategories();

            return View(categories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}