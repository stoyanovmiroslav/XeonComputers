using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Services.Contracts;
using XeonComputers.Areas.Administrator.ViewModels.ParentCategory;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ParentCategoriesController : AdministratorController
    {
        private IParentCategoryService parentCategoryService;

        public ParentCategoriesController(IParentCategoryService parentCategoryService)
        {
            this.parentCategoryService = parentCategoryService;
        }

        public IActionResult Edit(int id)
        {
            ParentCategory category = this.parentCategoryService.GetParentCategoryById(id);

            if (category == null)
            {
                return RedirectToAction("All");
            }

            var categoryViewModel = new ParentCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ParentCategoryViewModel model)
        {
            this.parentCategoryService.EditParentCategory(model.Id, model.Name);

            return RedirectToAction("All");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ParentCategoryViewModel model)
        {
            this.parentCategoryService.AddMainCategory(model.Name);

            return RedirectToAction("All");
        }

        public IActionResult All()
        {
            var categories = this.parentCategoryService.GetParentCategories();

            var categoriesViewModel = categories.Select(x => new ParentCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ChildCategoriesCount = x.ChildCategories.Count
            }).ToArray();

            return View(categoriesViewModel);
        }

        public IActionResult Delete(int id)
        {
            this.parentCategoryService.DeleteParentCategory(id);

            return RedirectToAction(nameof(All));
        }
    }
}