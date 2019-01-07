using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Services.Contracts;
using XeonComputers.Areas.Administrator.ViewModels;
using XeonComputers.Areas.Administrator.ViewModels.ChildCategory;
using XeonComputers.Areas.Administrator.ViewModels.ParentCategory;
using XeonComputers.Common;
using XeonComputers.Models;
using AutoMapper;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ChildCategoriesController : AdministratorController
    {
        private const string CANNOT_DELETE_CATEGORY_IF_ANY_PRODUCTS = "Може да изтриете категория само ако не съдържа продукти!";

        private readonly IChildCategoriesService childCategoryService;
        private readonly IParentCategoriesService parentCategoryService;
        private readonly IImagesService imageService;
        private readonly IMapper mapper;

        public ChildCategoriesController(IChildCategoriesService childCategoryService,
                                         IParentCategoriesService parentCategoryService,
                                         IImagesService imageService,
                                         IMapper mapper)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        public IActionResult Edit(int id)
        {
            var category = this.childCategoryService.GetChildCategoryById(id);
            var parentCategories = this.parentCategoryService.GetParentCategories().ToList();

            if (category == null)
            {
                return RedirectToAction(nameof(All));
            }

            var categoryViewModel = mapper.Map<EditChildCategoryViewModel>(category);
            categoryViewModel.ParentCategories = parentCategories;

            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditChildCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ParentCategories = this.parentCategoryService.GetParentCategories().ToList();

                return this.View(model);
            }

            this.childCategoryService.EditChildCategory(model.Id, model.Name, 
                                                        model.Description, (int)model.ParentCategoryId);

            if (model.FormImage != null)
            {
                var imagePath = string.Format(GlobalConstants.CHILD_CATEGORY_PATH_TEMPLATE, model.Id);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.FormImage.CopyToAsync(stream);
                }

                this.childCategoryService.AddImageUrl(model.Id);
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Add()
        {
            var parentCategories = this.parentCategoryService.GetParentCategories().ToList();

            var addViewModel = new AddChildCategoryViewModel
            {
                ParentCategories = parentCategories
            };

            return View(addViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddChildCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ParentCategories = this.parentCategoryService.GetParentCategories().ToList();

                return this.View(model);
            }

            var childCategory = this.childCategoryService
                                    .CreateChildCategory(model.Name, model.Description, (int)model.ParentId);

            if (model.FormImage != null)
            {
                var imagePath = string.Format(GlobalConstants.CHILD_CATEGORY_PATH_TEMPLATE, childCategory.Id);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.FormImage.CopyToAsync(stream);
                }

                this.childCategoryService.AddImageUrl(childCategory.Id);
            }

            return this.RedirectToAction(nameof(All));
        }

        public IActionResult All()
        {
            var childCategories = this.childCategoryService.GetChildCategories();
            var childCategoriesViewModel = this.mapper.Map<IList<AllChildCategoryViewModel>>(childCategories);

            var parentCategories = this.parentCategoryService.GetParentCategories();
            var parentCategoriesViewModel = this.mapper.Map<IList<ParentCategoryViewModel>>(parentCategories);

            var childParentViewModel = new AllChildParentCategoryViewModel
            {
                ChildCategoryViewModel = childCategoriesViewModel,
                ParentCategoryViewModels = parentCategoriesViewModel
            };

            return View(childParentViewModel);
        }

        public IActionResult Delete(int id)
        {
            if (!this.childCategoryService.DeleteChildCategory(id))
            {
                this.TempData["error"] = CANNOT_DELETE_CATEGORY_IF_ANY_PRODUCTS;
            }

            return RedirectToAction(nameof(All));
        }
    }
}