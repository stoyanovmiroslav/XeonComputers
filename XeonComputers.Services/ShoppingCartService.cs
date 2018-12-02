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
    public class ShoppingCartService : IShoppingCartService
    {
        private const int DEFAULT_PRODUCT_QUANTITY = 1;

        private readonly XeonDbContext db;
        private readonly IProductService productService;
        private readonly UserManager<XeonUser> userManager;

        public ShoppingCartService(XeonDbContext db, 
                                  IProductService productService,
                                  UserManager<XeonUser> userManager)
        {
            this.db = db;
            this.productService = productService;
            this.userManager = userManager;
        }

        public void AddProductInShoppingCart(int productId, string username)
        {
            var product = this.productService.GetProductById(productId);
            var user = this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();

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
                Quantity = DEFAULT_PRODUCT_QUANTITY,
                ShoppingCartId = user.ShoppingCartId
            };

            this.db.ShoppingCartProducts.Add(shoppingCartProduct);
            this.db.SaveChanges();
        }

        public void DeleteProductFromShoppingCart(int id, string name)
        {
            var product = this.productService.GetProductById(id);
            var user = this.userManager.FindByNameAsync(name).GetAwaiter().GetResult();

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCart = GetShoppingCartProduct(product.Id, user.ShoppingCartId);

            this.db.ShoppingCartProducts.Remove(shoppingCart);
            this.db.SaveChanges();
        }

        public void EditProductInShoppingCart(int productId, string username, int quantity)
        {
            var product = this.productService.GetProductById(productId);
            var user = this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();

            if (product == null || user == null || quantity <= 0)
            {
                return;
            }

            var shoppingCartProduct = GetShoppingCartProduct(productId, user.ShoppingCartId);
            if (shoppingCartProduct == null)
            {
                return;
            }

            shoppingCartProduct.Quantity = quantity;

            this.db.Update(shoppingCartProduct);
            this.db.SaveChanges();
        }

        public List<ShoppingCartProduct> GetAllShoppingCartProducts(string username)
        {
            var user = this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();

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
