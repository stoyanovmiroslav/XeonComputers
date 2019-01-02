using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class ShoppingCartsService : IShoppingCartsService
    {
        private const int DEFAULT_PRODUCT_QUANTITY = 1;

        private readonly XeonDbContext db;
        private readonly IProductsService productService;
        private readonly IUsersService userService;

        public ShoppingCartsService(XeonDbContext db, 
                                  IProductsService productService,
                                  IUsersService userService)
        {
            this.db = db;
            this.productService = productService;
            this.userService = userService;
        }

        public void AddProductInShoppingCart(int productId, string username, int? quntity = null)
        {
            var product = this.productService.GetProductById(productId);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCartProduct = GetShoppingCartProduct(productId, user.ShoppingCartId);
         
            if (shoppingCartProduct != null)
            {
                return;   
            }

            shoppingCartProduct = new ShoppingCartProduct
            {
                Product = product,
                Quantity = quntity == null ? DEFAULT_PRODUCT_QUANTITY : quntity.Value,
                ShoppingCartId = user.ShoppingCartId
            };

            this.db.ShoppingCartProducts.Add(shoppingCartProduct);
            this.db.SaveChanges();
        }

        public bool AnyProducts(string username)
        {
            return this.db.ShoppingCartProducts.Any(x => x.ShoppingCart.User.UserName == username);
        }

        public void DeleteAllProductFromShoppingCart(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return;
            }

            var shoppingCartProducts = this.db.ShoppingCartProducts.Where(x => x.ShoppingCartId == user.ShoppingCartId);

            this.db.ShoppingCartProducts.RemoveRange(shoppingCartProducts);
            this.db.SaveChanges();
        }

        public void DeleteProductFromShoppingCart(int id, string username)
        {
            var product = this.productService.GetProductById(id);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCart = GetShoppingCartProduct(product.Id, user.ShoppingCartId);

            this.db.ShoppingCartProducts.Remove(shoppingCart);
            this.db.SaveChanges();
        }

        public void EditProductQuantityInShoppingCart(int productId, string username, int quantity)
        {
            var product = this.productService.GetProductById(productId);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null || quantity <= 0)
            {
                return;
            }

            var shoppingCartProduct = this.GetShoppingCartProduct(productId, user.ShoppingCartId);
            if (shoppingCartProduct == null)
            {
                return;
            }

            shoppingCartProduct.Quantity = quantity;

            this.db.Update(shoppingCartProduct);
            this.db.SaveChanges();
        }

        public IEnumerable<ShoppingCartProduct> GetAllShoppingCartProducts(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            return this.db.ShoppingCartProducts.Include(x => x.Product)
                                               .ThenInclude(x => x.Images)
                                               .Include(x => x.ShoppingCart)
                                               .Where(x => x.ShoppingCart.User.UserName == username).ToList();
        }

        private ShoppingCartProduct GetShoppingCartProduct(int productId, int shoppingCartId)
        {
            return this.db.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId);
        }
    }
}