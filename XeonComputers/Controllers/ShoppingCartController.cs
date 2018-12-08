using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private const int DEFAULT_PRODUCT_QUANTITY = 1;

        private readonly IShoppingCartService shoppingCartService;
        private readonly IMemoryCache cache;
        private readonly IProductsService productSevice;


        public ShoppingCartController(IShoppingCartService shoppingCartService, IMemoryCache cache,
                                      IProductsService productSevice)
        {
            this.shoppingCartService = shoppingCartService;
            this.cache = cache;
            this.productSevice = productSevice;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(this.User.Identity.Name);

                var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new AllFavoriteViewModel
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

            var cart = SessionHelper.GetObjectFromJson<List<AllFavoriteViewModel>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<AllFavoriteViewModel>();
            }

            return this.View(cart);
        }

        public IActionResult Add(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name);

                return this.RedirectToAction("Index");
            }

            List<AllFavoriteViewModel> cart = SessionHelper.GetObjectFromJson<List<AllFavoriteViewModel>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<AllFavoriteViewModel>();
            }

            if (!cart.Any(x => x.Id == id))
            {
                var productViewModel = this.productSevice.GetProductById(id);
                cart.Add(new AllFavoriteViewModel
                {
                    Id = id,
                    ImageUrl = productViewModel.Images.FirstOrDefault()?.ImageUrl,
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Quantity = DEFAULT_PRODUCT_QUANTITY,
                    TotalPrice = DEFAULT_PRODUCT_QUANTITY * productViewModel.Price
                });

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return this.RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.DeleteProductFromShoppingCart(id, this.User.Identity.Name);

                return this.RedirectToAction("Index");
            }

            List<AllFavoriteViewModel> cart = SessionHelper.GetObjectFromJson<List<AllFavoriteViewModel>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<AllFavoriteViewModel>();
            }

            if (cart.Any(x => x.Id == id))
            {
                var product = cart.First(x => x.Id == id);
                cart.Remove(product);

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return this.RedirectToAction("Index");
        }

        public IActionResult Edit(int id, int quantity)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.EditProductInShoppingCart(id, this.User.Identity.Name, quantity);

                return this.RedirectToAction("Index");
            }

            List<AllFavoriteViewModel> cart = SessionHelper.GetObjectFromJson<List<AllFavoriteViewModel>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<AllFavoriteViewModel>();
            }

            if (cart.Any(x => x.Id == id) && quantity >= 0)
            {
                var product = cart.First(x => x.Id == id);
                product.Quantity = quantity;
                product.TotalPrice = quantity * product.Price;

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return this.RedirectToAction("Index");
        }
    }
}