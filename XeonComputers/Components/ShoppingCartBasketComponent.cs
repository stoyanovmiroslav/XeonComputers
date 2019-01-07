using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Common;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Components
{
    public class ShoppingCartBasketComponent : ViewComponent
    {
        private IShoppingCartsService shoppingCartService;

        public ShoppingCartBasketComponent(IShoppingCartsService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IViewComponentResult Invoke()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                bool isPartnerOrAdmin = this.User.IsInRole(Role.Admin.ToString()) || this.User.IsInRole(Role.Partner.ToString());

                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(this.User.Identity.Name);

                var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new ShoppingCartProductsViewModel
                {
                    Id = x.ProductId,
                    ImageUrl = x.Product.Images.FirstOrDefault()?.ImageUrl,
                    Name = x.Product.Name,
                    Price = isPartnerOrAdmin ? x.Product.ParnersPrice : x.Product.Price,
                    Quantity = x.Quantity,
                    TotalPrice = x.Quantity * (isPartnerOrAdmin ? x.Product.ParnersPrice : x.Product.Price)
                }).ToList();

                return this.View(shoppingCartProductsViewModel);
            }

            var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
            if (shoppingCartSession == null)
            {
                shoppingCartSession = new List<ShoppingCartProductsViewModel>();
            }

            return this.View(shoppingCartSession);
        }
    }
}