using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class ShoppingCartsServiceTests
    {
        [Fact]
        public void AddProductInShoppingCartShouldAddProductInShoppingCart()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "AddProductInShoppingCart_ShoppingCart_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username, ShoppingCart = new ShoppingCart() };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == username));

            var productId = 1;
            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(productId))
                          .Returns(new Product { Name = "USB Cable" });

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.AddProductInShoppingCart(productId, username);

            var shoppingCartProducts = dbContext.ShoppingCartProducts.ToList();

            Assert.Single(shoppingCartProducts);
            Assert.Equal(user.ShoppingCartId, shoppingCartProducts.First().ShoppingCartId);
        }

        [Fact]
        public void AddProductInShoppingCartWithInvalidUserShouldNotAddProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "AddProductInShoppingCartWithInvalidUser_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            XeonUser user = null;

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var productId = 1;
            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(productId))
                          .Returns(new Product { Name = "USB Cable" });

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.AddProductInShoppingCart(productId, username);

            var shoppingCartProducts = dbContext.ShoppingCartProducts.ToList();

            Assert.Empty(shoppingCartProducts);
        }

        [Fact]
        public void AddProductInShoppingCartWithInvalidProductShouldNotAddProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "AddProductInShoppingCartWithInvalidProduct_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username, ShoppingCart = new ShoppingCart() };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == username));

            var productId = 1;
            Product product = null;
            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(productId))
                          .Returns(product);

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.AddProductInShoppingCart(productId, username);

            var shoppingCartProducts = dbContext.ShoppingCartProducts.ToList();

            Assert.Empty(shoppingCartProducts);
        }

        [Fact]
        public void AddProductInShoppingCartWhithExistingProductShouldNotAddProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "AddProductInShoppingCartExistingProduct_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var user = new XeonUser { UserName = "user@gmail.com", ShoppingCart = new ShoppingCart() };
            dbContext.Users.Add(user);

            var product = new Product { Name = "USB Cable" };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(product.Id))
                          .Returns(product);

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.AddProductInShoppingCart(product.Id, user.UserName);

            shoppingCartsService.AddProductInShoppingCart(product.Id, user.UserName);

            var shoppingCartProducts = dbContext.ShoppingCartProducts.ToList();

            Assert.Single(shoppingCartProducts);
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(3, 3)]
        [InlineData(7, 7)]
        public void AddProductInShoppingCartShouldAddDefaultOrSubmittedProductQuantity(int? quantity, int expectedQuantity)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"AddProductInShoppingCartWhithQuantity_{expectedQuantity}_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var user = new XeonUser { UserName = "user@gmail.com", ShoppingCart = new ShoppingCart() };
            dbContext.Users.Add(user);

            var product = new Product { Name = "USB Cable" };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(product.Id))
                          .Returns(product);

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.AddProductInShoppingCart(product.Id, user.UserName, quantity);

            var shoppingCartProducts = dbContext.ShoppingCartProducts.ToList();

            Assert.Equal(expectedQuantity, shoppingCartProducts.First().Quantity);
        }

        [Fact]
        public void AnyProductsShouldReturnTrueWhenThereAreProducts()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"АnyProductsTrue_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var user = new XeonUser { UserName = "user@gmail.com", ShoppingCart = new ShoppingCart() };
            dbContext.Users.Add(user);

            var product = new Product { Name = "USB Cable" };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(product.Id))
                          .Returns(product);

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.AddProductInShoppingCart(product.Id, user.UserName);

            var areThereAnyProducts = shoppingCartsService.AnyProducts(user.UserName);

            Assert.True(areThereAnyProducts);
        }

        [Fact]
        public void AnyProductsShouldReturnFalseWhenThereAreNotAnyProducts()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"АnyProductsFalse_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var user = new XeonUser { UserName = "user@gmail.com", ShoppingCart = new ShoppingCart() };
            dbContext.Users.Add(user);

            var product = new Product { Name = "USB Cable" };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productService = new Mock<IProductsService>();

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);
            var areThereAnyProducts = shoppingCartsService.AnyProducts(user.UserName);

            Assert.False(areThereAnyProducts);
        }

        [Fact]
        public void GetAllShoppingCartProductsShouldReturnAllShoppingCartProductsForUser()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"GetAllShoppingCartProducts_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var products = new List<Product>
            {
               new Product { Name = "USB 1.0" },
               new Product { Name = "USB 2.0" },
               new Product { Name = "USB 3.0" },
               new Product { Name = "USB 4.0" }
            };
            dbContext.Products.AddRange(products);

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
               new ShoppingCartProduct { Product = products.First() },
               new ShoppingCartProduct { Product = products.Last() },
            };

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                ShoppingCart = new ShoppingCart
                {
                    ShoppingCartProducts = shoppingCartProducts
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            var shoppingCatrProducts = shoppingCartsService.GetAllShoppingCartProducts(user.UserName);

            Assert.Equal(2, shoppingCatrProducts.Count());
        }

        [Fact]
        public void GetAllShoppingCartProductsWhithInvalidUserShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"GetAllShoppingCartProductsNull_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            XeonUser user = null;

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            var shoppingCatrProducts = shoppingCartsService.GetAllShoppingCartProducts(username);

            Assert.Null(shoppingCatrProducts);
        }

        [Fact]
        public void DeleteAllProductFromShoppingCartShoulDeleteAllShoppingCartProductsForCurrentUser()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"DeleteAllProductFromShoppingCart_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var products = new List<Product>
            {
               new Product { Name = "USB 1.0" },
               new Product { Name = "USB 2.0" },
               new Product { Name = "USB 3.0" },
               new Product { Name = "USB 4.0" }
            };
            dbContext.Products.AddRange(products);

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
               new ShoppingCartProduct { Product = products.First() },
               new ShoppingCartProduct { Product = products.Last() },
            };

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                ShoppingCart = new ShoppingCart
                {
                    ShoppingCartProducts = shoppingCartProducts
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.DeleteAllProductFromShoppingCart(user.UserName);

            var userShoppingCart = dbContext.ShoppingCartProducts.Where(x => x.ShoppingCartId == user.ShoppingCartId);

            Assert.Empty(userShoppingCart);
        }

        [Fact]
        public void DeleteProductFromShoppingCartShoulDeleteExactProductFromCurrentUser()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: $"DeleteProductFromShoppingCart_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var products = new List<Product>
            {
               new Product { Name = "USB 1.0" },
               new Product { Name = "USB 2.0" },
               new Product { Name = "USB 3.0" },
               new Product { Name = "USB 4.0" }
            };
            dbContext.Products.AddRange(products);

            var shoppingCartProducts = new List<ShoppingCartProduct>
            {
               new ShoppingCartProduct { Product = products.First() },
               new ShoppingCartProduct { Product = products.Last() },
            };

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                ShoppingCart = new ShoppingCart
                {
                    ShoppingCartProducts = shoppingCartProducts
                }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(user.UserName))
                       .Returns(user);

            var productForDelete = products.First();
            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(productForDelete.Id))
                          .Returns(productForDelete);

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.DeleteProductFromShoppingCart(productForDelete.Id, user.UserName);

            var shoppingCartProduct = dbContext.ShoppingCartProducts.Where(x => x.ShoppingCartId == user.ShoppingCartId && x.ProductId == productForDelete.Id).ToList();

            Assert.Empty(shoppingCartProduct);
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(7, 7)]
        public void EditProductQuantityInShoppingCartShouldEditProductQuantity(int quantity, int expectedQuantity)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "EditProductQuantityInShoppingCart_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var product = new Product { Name = "USB 1.0" };
            dbContext.Products.AddRange(product);

            var shoppingCart = new ShoppingCart
            {
                ShoppingCartProducts = new List<ShoppingCartProduct>
                {
                   new ShoppingCartProduct { Product = product, Quantity = 1 },
                }
            };

            var username = "user@gmail.com";
            var user = new XeonUser
            {
                UserName = username,
                ShoppingCart = shoppingCart
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var productService = new Mock<IProductsService>();
            productService.Setup(p => p.GetProductById(product.Id))
                          .Returns(product);

            var shoppingCartsService = new ShoppingCartsService(dbContext, productService.Object, userService.Object);

            shoppingCartsService.EditProductQuantityInShoppingCart(product.Id, username, quantity);

            var shoppingCartProduct = dbContext.ShoppingCartProducts
                                               .FirstOrDefault(x => x.ProductId == product.Id
                                                     && x.ShoppingCartId == user.ShoppingCartId);

            Assert.Equal(expectedQuantity, shoppingCartProduct.Quantity);
        }
    }
}