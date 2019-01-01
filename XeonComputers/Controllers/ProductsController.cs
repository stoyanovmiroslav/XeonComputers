using AutoMapper;
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
        private readonly IProductsService productService;
        private readonly IMapper mapper;

        public ProductsController(IProductsService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        public IActionResult Details(int id)
        {
            var product = this.productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = mapper.Map<DetailsProductViewModel>(product);

            return View(productViewModel);
        }

        public IActionResult Rate(int rating, int productId)
        {
            this.productService.AddReview(rating, productId);

            return RedirectToAction(nameof(Details), new { id = productId });
        }
    }
}