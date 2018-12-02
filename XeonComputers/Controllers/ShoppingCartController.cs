using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(this.User.Identity.Name);


            var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new IndexShoppingCartProductsViewModel
                                                                    {
                                                                         Id = x.ProductId,
                                                                         ImageUrl = x.Product.Images.FirstOrDefault()?.ImageUrl,
                                                                         Name = x.Product.Name,
                                                                         Price = x.Product.Price,
                                                                         Quantity = x.Quantity,
                                                                         TotalPrice = x.Quantity * x.Product.Price
                                                                    }).ToList();

            return this.View(shoppingCartProductsViewModel);
        }


        public IActionResult Add(int id)
        {
            this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name);

            return this.RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.shoppingCartService.DeleteProductFromShoppingCart(id, this.User.Identity.Name);

            return this.RedirectToAction("Index");
        }

        public IActionResult Edit(int id, int quantity)
        {
            this.shoppingCartService.EditProductInShoppingCart(id, this.User.Identity.Name, quantity);

            return this.RedirectToAction("Index");
        }
    }
}
