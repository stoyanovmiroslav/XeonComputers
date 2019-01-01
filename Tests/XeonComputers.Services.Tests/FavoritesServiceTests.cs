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
    public class FavoritesServiceTests
    {
        [Fact]
        public void AddFavoriteProductShouldAddFavoriteProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Add_Favorites_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@abv.bg";
            dbContext.Users.Add(new XeonUser { UserName = username });

            var productName = "USB";
            dbContext.Products.Add(new Product { Name = productName });
            dbContext.SaveChanges();

            var product = dbContext.Products.FirstOrDefault(x => x.Name == productName);

            var favoriteService = new FavoritesService(dbContext);

            favoriteService.Add(product.Id, username);

            var favoriteProducts = dbContext.Users
                                            .Include(x => x.FavoriteProducts)
                                            .FirstOrDefault(x => x.UserName == username)
                                            .FavoriteProducts;

            var favoriteProductsCount = favoriteProducts.Count();
            var favoriteProduct = favoriteProducts.FirstOrDefault(x => x.Product.Name == productName);

            Assert.Equal(1, favoriteProductsCount);
            Assert.Equal(productName, favoriteProduct.Product.Name);
        }

        [Fact]
        public void AddFavoriteProductWithInvalidUserShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Add_Favorites_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var invalidUsername = "user@abv.bg";

            var productName = "USB";
            dbContext.Products.Add(new Product { Name = productName });
            dbContext.SaveChanges();

            var product = dbContext.Products.FirstOrDefault(x => x.Name == productName);

            var favoriteService = new FavoritesService(dbContext);

            var isAddFavoriteProduct = favoriteService.Add(product.Id, invalidUsername);

            Assert.False(isAddFavoriteProduct);
        }

        [Fact]
        public void AddFavoriteProductWithInvalidProductShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Add_Favorites_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@abv.bg";
            dbContext.Users.Add(new XeonUser { UserName = username });
            dbContext.SaveChanges();
            
            var favoriteService = new FavoritesService(dbContext);
            var invalidProductId = 123;
            var isAddFavoriteProduct = favoriteService.Add(invalidProductId, username);

            Assert.False(isAddFavoriteProduct);
        }

        [Fact]
        public void AddFavoriteProductWhithExistingProductShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "AddExisting_Favorites_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@abv.bg";
            dbContext.Users.Add(new XeonUser { UserName = username });

            var productName = "USB";
            dbContext.Products.Add(new Product { Name = productName });
            dbContext.SaveChanges();

            var product = dbContext.Products.FirstOrDefault(x => x.Name == productName);

            var favoriteService = new FavoritesService(dbContext);

            favoriteService.Add(product.Id, username);
            var isAddFavoriteProduct = favoriteService.Add(product.Id, username);

            var favoriteProducts = dbContext.Users
                                            .Include(x => x.FavoriteProducts)
                                            .FirstOrDefault(x => x.UserName == username)
                                            .FavoriteProducts;

            var favoriteProductsCount = favoriteProducts.Count();

            Assert.False(isAddFavoriteProduct);
            Assert.Equal(1, favoriteProductsCount);
        }

        [Fact]
        public void AllFavoriteShouldReturnAllFavoriteProducts()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                          .UseInMemoryDatabase(databaseName: "All_Favorites_Database")
                          .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@abv.bg";
            dbContext.Users.Add(new XeonUser { UserName = username });

            var products = new List<Product>
                           {
                               new Product { Name = "USB" },
                               new Product { Name = "Phone Samsung" },
                               new Product { Name = "Phone Nokia" },
                               new Product { Name = "Phone Iphone" },
                               new Product { Name = "Tablet Galaxy" }
                           };

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            var favoriteService = new FavoritesService(dbContext);

            foreach (var product in products.Take(3))
            {
                favoriteService.Add(product.Id, username);
            }

            var favoriteProducts = favoriteService.All(username);

            Assert.Equal(3, favoriteProducts.Count());
        }

        [Fact]
        public void DeleteShouldDeleteFavoriteProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                          .UseInMemoryDatabase(databaseName: "Delete_Favorites_Database")
                          .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@abv.bg";
            dbContext.Users.Add(new XeonUser { UserName = username });


            var productNameForDelete = "USB";
            var products = new List<Product>
                           {
                               new Product { Name = productNameForDelete },
                               new Product { Name = "Phone Samsung" },
                               new Product { Name = "Phone Nokia" },
                               new Product { Name = "Phone Iphone" },
                               new Product { Name = "Tablet Galaxy" }
                           };

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            var favoriteService = new FavoritesService(dbContext);

            foreach (var product in products.Take(3))
            {
                favoriteService.Add(product.Id, username);
            }

            var productId = products.FirstOrDefault(x => x.Name == productNameForDelete).Id;
            favoriteService.Delete(productId, username);

            var userFavoriteProduxts = dbContext.Users
                                                .FirstOrDefault(x => x.UserName == username)
                                                .FavoriteProducts
                                                .ToList();

            var isProductExist = userFavoriteProduxts.Any(x => x.Product.Name == productNameForDelete);

            Assert.Equal(2, userFavoriteProduxts.Count());
            Assert.False(isProductExist);
        }
    }
}