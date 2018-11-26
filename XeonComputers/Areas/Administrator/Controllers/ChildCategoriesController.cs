using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Areas.Administrator.Services.Contracts;
using XeonComputers.Areas.Administrator.ViewModels;
using XeonComputers.Areas.Administrator.ViewModels.ChildCategory;
using XeonComputers.Areas.Administrator.ViewModels.ParentCategory;
using XeonComputers.Common;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class ChildCategoriesController : AdministratorController
    {
        private IChildCategoryService childCategoryService;
        private IParentCategoryService parentCategoryService;
        private IImageService imageService;

        public ChildCategoriesController(IChildCategoryService childCategoryService,
                                         IParentCategoryService parentCategoryService,
                                         IImageService imageService)
        {
            this.childCategoryService = childCategoryService;
            this.parentCategoryService = parentCategoryService;
            this.imageService = imageService;
        }

        public IActionResult Edit(int id)
        {
            var category = this.childCategoryService.GetChildCategoryById(id);
            var parentCategory = this.parentCategoryService.GetParentCategories();

            if (category == null)
            {
                return RedirectToAction(nameof(All));
            }

            var categoryViewModel = new EditChildCategoryViewModel
            {
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentCategoryId,
                ParentCategories = parentCategory,
            };

            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditChildCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ParentCategories = this.parentCategoryService.GetParentCategories();

                return this.View(model);
            }

            this.childCategoryService.EditChildCategory(model.Id, model.Name, 
                                                        model.Description, (int)model.ParentId);

            if (model.FormImage != null)
            {
                var imagePath = string.Format(GlobalConstans.CHILD_CATEGORY_PATH_TEMPLATE, model.Id);
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
            var parentCategories = this.parentCategoryService.GetParentCategories();

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
                model.ParentCategories = this.parentCategoryService.GetParentCategories();

                return this.View(model);
            }

            var childCategory = this.childCategoryService
                                    .CreateChildCategory(model.Name, model.Description, (int)model.ParentId);

            if (model.FormImage != null)
            {
                var imagePath = string.Format(GlobalConstans.CHILD_CATEGORY_PATH_TEMPLATE, childCategory.Id);
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
            var childCategoriesViewModel = childCategories.Select(x => new AllChildCategoryViewModel
                                                                  {
                                                                       Name = x.Name,
                                                                       Description = x.Description,
                                                                       Id = x.Id,
                                                                       ProductsCount = x.Products.Count,
                                                                       ParentCategoryName = x.ParentCategory.Name
                                                                  }).ToArray();

            var parentCategories = this.parentCategoryService.GetParentCategories();
            var parentCategoriesViewModel = parentCategories.Select(x => new ParentCategoryViewModel
                                                                    {
                                                                        Id = x.Id,
                                                                        Name = x.Name
                                                                    }).ToArray();

            var childParentViewModel = new AllChildParentCategoryViewModel
            {
                ChildCategoryViewModel = childCategoriesViewModel,
                ParentCategoryViewModels = parentCategoriesViewModel
            };

            return View(childParentViewModel);
        }

        public IActionResult Delete(int id)
        {
            this.childCategoryService.DeleteChildCategory(id);

            return RedirectToAction(nameof(All));
        }
    }
}