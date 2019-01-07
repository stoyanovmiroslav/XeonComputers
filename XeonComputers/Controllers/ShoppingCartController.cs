using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;
using System.ComponentModel;
using XeonComputers.Common;
using XeonComputers.Models.Enums;
using AutoMapper;
using XeonComputers.Areas.Administrator.ViewModels.Suppliers;

namespace XeonComputers.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private const int DEFAULT_PRODUCT_QUANTITY = 1;

        private readonly IShoppingCartsService shoppingCartService;
        private readonly IProductsService productSevice;
        private readonly ISuppliersService suppliersService;
        private readonly IMapper mapper;


        public ShoppingCartController(IShoppingCartsService shoppingCartService,
                                      IProductsService productSevice,
                                      ISuppliersService suppliersService,
                                      IMapper mapper)
        {
            this.shoppingCartService = shoppingCartService;
            this.productSevice = productSevice;
            this.suppliersService = suppliersService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(this.User.Identity.Name);
                if (shoppingCartProducts.Count() == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                bool isPartnerOrAdmin = this.User.IsInRole(Role.Admin.ToString()) || this.User.IsInRole(Role.Partner.ToString());
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
            if (shoppingCartSession == null || shoppingCartSession.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return this.View(shoppingCartSession);
        }

        public IActionResult Add(int id, bool direct)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name);
            }
            else
            {
                List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
                if (shoppingCartSession == null)
                {
                    shoppingCartSession = new List<ShoppingCartProductsViewModel>();
                }

                if (!shoppingCartSession.Any(x => x.Id == id))
                {
                    var product = this.productSevice.GetProductById(id);

                    var shoppingCart = mapper.Map<ShoppingCartProductsViewModel>(product);
                    shoppingCart.Quantity = DEFAULT_PRODUCT_QUANTITY;
                    shoppingCart.TotalPrice = shoppingCart.Quantity * shoppingCart.Price;

                    shoppingCartSession.Add(shoppingCart);

                    SessionHelper.SetObjectAsJson(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY, shoppingCartSession);
                }
            }

            if (direct == true)
            {
                return this.RedirectToAction("Create", "Orders");
            }

            return this.RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.DeleteProductFromShoppingCart(id, this.User.Identity.Name);

                return this.RedirectToAction(nameof(Index));
            }

            List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
            if (shoppingCartSession == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (shoppingCartSession.Any(x => x.Id == id))
            {
                var product = shoppingCartSession.First(x => x.Id == id);
                shoppingCartSession.Remove(product);

                SessionHelper.SetObjectAsJson(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY, shoppingCartSession);
            }

            return this.RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id, int quantity)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.EditProductQuantityInShoppingCart(id, this.User.Identity.Name, quantity);

                return this.RedirectToAction(nameof(Index));
            }

            List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
            if (shoppingCartSession == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (shoppingCartSession.Any(x => x.Id == id) && quantity > 0)
            {
                var product = shoppingCartSession.First(x => x.Id == id);
                product.Quantity = quantity;
                product.TotalPrice = quantity * product.Price;

                SessionHelper.SetObjectAsJson(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY, shoppingCartSession);
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}