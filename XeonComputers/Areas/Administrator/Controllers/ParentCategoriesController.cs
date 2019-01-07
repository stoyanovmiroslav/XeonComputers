using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Services.Contracts;
using XeonComputers.Areas.Administrator.ViewModels.ParentCategory;
using XeonComputers.Models;
using AutoMapper;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ParentCategoriesController : AdministratorController
    {
        private const string CANNOT_DELETE_CATEGORY_IF_ANY_CHILD_CATEGORY = "Може да изтриете основна категория само ако не съдържа други категории!";

        private readonly IParentCategoriesService parentCategoryService;
        private readonly IMapper mapper;

        public ParentCategoriesController(IParentCategoriesService parentCategoryService, IMapper mapper)
        {
            this.parentCategoryService = parentCategoryService;
            this.mapper = mapper;
        }

        public IActionResult Edit(int id)
        {
            ParentCategory category = this.parentCategoryService.GetParentCategoryById(id);

            if (category == null)
            {
                return RedirectToAction("All");
            }

            var categoryViewModel = mapper.Map<ParentCategoryViewModel>(category);

            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ParentCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

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
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.parentCategoryService.CreateParentCategory(model.Name);

            return RedirectToAction("All");
        }

        public IActionResult All()
        {
            var categories = this.parentCategoryService.GetParentCategories();

            var categoriesViewModel = mapper.Map<IList<ParentCategoryViewModel>>(categories);

            return View(categoriesViewModel);
        }

        public IActionResult Delete(int id)
        {
            if (!this.parentCategoryService.DeleteParentCategory(id))
            {
                this.TempData["error"] = CANNOT_DELETE_CATEGORY_IF_ANY_CHILD_CATEGORY;
            }

            return RedirectToAction(nameof(All));
        }
    }
}