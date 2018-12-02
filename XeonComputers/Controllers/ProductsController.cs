using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Products;

namespace XeonComputers.Controllers
{
    public class ProductsController : BaseController
    {
        private IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }


        public IActionResult Details(int id)
        {
            var product = this.productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new DetailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ChildCategoryName = product.ChildCategory.Name,
                Price = product.Price,
                ParnersPrice = product.ParnersPrice,
                Specification = product.Specification,
                ImageUrls = product.Images.Select(x => x.ImageUrl).ToList()
            };


            return View(productViewModel);
        }
    }
}