using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Components
{
    public class ShoppingCartComponent : ViewComponent
    {
        private IShoppingCartsService shoppingCartService;

        public ShoppingCartComponent(IShoppingCartsService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IViewComponentResult Invoke()
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
    }
}