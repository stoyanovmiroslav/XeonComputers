using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Components
{
    public class ShoppingCartComponent : ViewComponent
    {
        private IShoppingCartService shoppingCartService;

        public ShoppingCartComponent(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IViewComponentResult Invoke()
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
    }
}