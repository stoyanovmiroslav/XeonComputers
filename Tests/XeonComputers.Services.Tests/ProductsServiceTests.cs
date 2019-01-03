using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class ProductsServiceTests
    {
        [Theory]
        [InlineData(4, 1)]
        [InlineData(6, 0)]
        [InlineData(-1, 0)]
        public void AddReviewShouldAddReview(int rating, int expected)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviews_Product_Database")
                        .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            var product = new Product { Name = "USB ", ChildCategory = childCategory };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            productService.AddReview(rating, product.Id);

            Assert.Equal(expected, product.Reviews.Count());
        }

        [Fact]
        public void AddProductShouldAddProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Add_Product_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var productService = new ProductsService(dbContext);

            var product = new Product { Name = "USB" };
            productService.AddProduct(product);

            var products = dbContext.Products.ToList();

            Assert.Single(products);
            Assert.Equal(product.Name, products.First().Name);
        }

        [Fact]
        public void AddProductNullEntityShouldNotAddProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "AddNull_Product_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var productService = new ProductsService(dbContext);

            Product product = null;
            productService.AddProduct(product);

            var products = dbContext.Products.ToList();

            Assert.Empty(products);
        }

        [Fact]
        public void HideProductShouldChangeHiteToTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Remove_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var productName = "USB";
            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = productName },
                new Product { Name = "Cable" },
                new Product { Name = "Keyboard" },
                new Product { Name = "Computer" },
            });
            dbContext.SaveChanges();

            var product = dbContext.Products.FirstOrDefault(x => x.Name == productName);

            var isProductHide = productService.HideProduct(product.Id);

            Assert.True(isProductHide);
            Assert.True(product.Hide);
        }

        [Fact]
        public void ShowProductShouldChangeHiteToFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Remove_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var productName = "USB";
            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = productName },
                new Product { Name = "Cable" },
                new Product { Name = "Keyboard" },
                new Product { Name = "Computer" },
            });
            dbContext.SaveChanges();

            var product = dbContext.Products.FirstOrDefault(x => x.Name == productName);

            var isProductShow = productService.ShowProduct(product.Id);

            Assert.True(isProductShow);
            Assert.False(product.Hide);
        }

        [Fact]
        public void HideInvalidProducShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "RemoveInvalid_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB" },
                new Product { Name = "Cable" },
                new Product { Name = "Keyboard" },
                new Product { Name = "Computer" },
            });
            dbContext.SaveChanges();

            var invalidProductId = 123;
            var isProductDeleted = productService.HideProduct(invalidProductId);

            Assert.False(isProductDeleted);
            Assert.Equal(4, dbContext.Products.Count());
        }

        [Fact]
        public void ProductExistsShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "ProductExists_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var productName = "USB";
            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = productName },
                new Product { Name = "Cable" },
                new Product { Name = "Keyboard" },
                new Product { Name = "Computer" },
            });
            dbContext.SaveChanges();

            var product = dbContext.Products.FirstOrDefault(x => x.Name == productName);

            var isProductExist = productService.ProductExists(product.Id);

            Assert.True(isProductExist);
        }

        [Fact]
        public void ProductExistsShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "ProductNotExists_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB" },
                new Product { Name = "Cable" },
                new Product { Name = "Keyboard" },
                new Product { Name = "Computer" },
            });
            dbContext.SaveChanges();

            var invalidProductId = 123;
            var isProductExist = productService.ProductExists(invalidProductId);

            Assert.False(isProductExist);
        }

        [Theory]
        [InlineData("USB", 1)]
        [InlineData("Monitor", 0)]
        [InlineData("cable", 2)]
        public void GetProductsBySearchShouldReturnGetProductsBySearch(string searchString, int expected)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                        .UseInMemoryDatabase(databaseName: $"{searchString}_GetProductsBySearch_Product_Database")
                        .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB Cable", ChildCategory = childCategory },
                new Product { Name = "Adapter Cable", ChildCategory = childCategory },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var products = productService.GetProductsBySearch(searchString);

            Assert.Equal(expected, products.Count());
        }

        [Fact]
        public void GetVisibleProductsShouldReturnAllVisibleProducts()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetProducts_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB", ChildCategory = childCategory },
                new Product { Name = "Cable", ChildCategory = childCategory },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var products = productService.GetVisibleProducts();

            Assert.Equal(4, products.Count());
            Assert.DoesNotContain(products, x => x.Hide == true);
        }

        [Fact]
        public void GetHideProductsShouldReturnAllHideProducts()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetHideProducts_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB", ChildCategory = childCategory, Hide = true },
                new Product { Name = "Cable", ChildCategory = childCategory, Hide = true },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var products = productService.GetHideProducts();

            Assert.Equal(2, products.Count());
            Assert.DoesNotContain(products, x => x.Hide == false);
        }

        [Fact]
        public void GetAllProductsShouldReturAllProducts()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetAllProducts_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB", ChildCategory = childCategory, Hide = true },
                new Product { Name = "Cable", ChildCategory = childCategory, Hide = true },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var products = productService.GetAllProducts();

            Assert.Equal(4, products.Count());
        }

        [Fact]
        public void GetProductByIdShouldReturnProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetProductById_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            var productName = "USB";
            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = productName, ChildCategory = childCategory },
                new Product { Name = "Cable", ChildCategory = childCategory },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var productId = dbContext.Products.FirstOrDefault(x => x.Name == productName).Id;
            var product = productService.GetProductById(productId);

            Assert.Equal(productName, product.Name);
        }

        [Fact]
        public void GetHideProductByIdShouldReturnOnlyHideProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetHideProductById_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "USB", ChildCategory = childCategory, Hide = true },
                new Product { Id = 2, Name = "Cable", ChildCategory = childCategory, Hide = false },
                new Product { Id = 3, Name = "Keyboard", ChildCategory = childCategory, Hide = false },
                new Product { Id = 4, Name = "Computer", ChildCategory = childCategory, Hide = false },
            };

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            var firstProduct = productService.GetHideProductById(products.First().Id);
            var lastProduct = productService.GetHideProductById(products.Last().Id);

            Assert.Equal(firstProduct.Name, products.First().Name);
            Assert.Null(lastProduct);
        }

        [Fact]
        public void EditProductProductShouldEditProduct()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                          .UseInMemoryDatabase(databaseName: "Add_Product_Database")
                          .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var product = new Product
            {
                Name = "USB",
                ParnersPrice = 31,
                Price = 39,
                Specification = "1.1"
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            product.Name = "NewName";
            product.ParnersPrice = 11;
            product.Price = 10;
            product.Specification = "2.0";
            productService.EditProduct(product);

            var editedProduct = dbContext.Products.FirstOrDefault(x => x.Name == product.Name);

            Assert.Equal(product.Name, editedProduct.Name);
            Assert.Equal(product.ParnersPrice, editedProduct.ParnersPrice);
            Assert.Equal(product.Price, editedProduct.Price);
            Assert.Equal(product.Specification, editedProduct.Specification);
        }

        [Fact]
        public void AddImageUrlsShouldAddImageUrls()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "AddImageUrls_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            var productName = "USB";
            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = productName, ChildCategory = childCategory },
                new Product { Name = "Cable", ChildCategory = childCategory },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var productId = dbContext.Products.FirstOrDefault(x => x.Name == productName).Id;
            var imageUrls = new List<string> { "wwwroot/image1", "wwwroot/image2", "wwwroot/image3" };
            productService.AddImageUrls(productId, imageUrls);

            var product = dbContext.Products.FirstOrDefault(x => x.Id == productId);

            Assert.Equal(3, product.Images.Count);
        }

        [Fact]
        public void GetImagesShouldReturnImageUrls()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetImages_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            var product = new Product
            {
                Name = "USB",
                ChildCategory = childCategory,
                Images = new List<Image>
                {
                   new Image { ImageUrl =  "wwwroot/image1"},
                   new Image { ImageUrl =  "wwwroot/image2"}
                }
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var productImagesCount = productService.GetImages(product.Id).Count();

            Assert.Equal(2, productImagesCount);
        }

        [Fact]
        public void GetImagesWhithInvalidProductShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetImagesNull_Product_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var invalidProductId = 123;
            var images = productService.GetImages(invalidProductId);

            Assert.Null(images);
        }

        [Fact]
        public void GetProductsByCategoryShouldReturnProductsByCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                        .UseInMemoryDatabase(databaseName: "GetProductsByCategory_Product_Database")
                        .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategory = new ChildCategory { Name = "Cables", ParentCategory = parentCategory };
            dbContext.ChildCategories.Add(childCategory);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB", ChildCategory = childCategory },
                new Product { Name = "Cable", ChildCategory = childCategory },
                new Product { Name = "Keyboard", ChildCategory = childCategory },
                new Product { Name = "Computer", ChildCategory = childCategory },
            });
            dbContext.SaveChanges();

            var products = productService.GetProductsByCategory(childCategory.Id);

            var invalidChildCategoryId = 123;
            var productsByInvalidId = productService.GetProductsByCategory(invalidChildCategoryId);

            Assert.Equal(4, products.Count());
            Assert.Empty(productsByInvalidId);
        }

        [Theory]
        [InlineData("1", 4, null, 1)]
        [InlineData("2", 1, "USB", null)]
        [InlineData("3", 0, "Asus", null)]
        [InlineData("4", 2, "cable", null)]
        [InlineData("5", 0, null, 123)]
        [InlineData("6", 5, null, null)]
        public void GetProductsFilterShouldFilterProducts(string test, int expected, string searchString, int? childCategoryId)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                        .UseInMemoryDatabase(databaseName: $"{test}_GetProductsFilter_Product_Database")
                        .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategories = new List<ChildCategory>
            {
               new ChildCategory { Name = "Cables", ParentCategory = parentCategory},
               new ChildCategory { Name = "Monitors", ParentCategory = parentCategory }
            };
            dbContext.ChildCategories.AddRange(childCategories);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB Cable", ChildCategory = childCategories.First() },
                new Product { Name = "Adapter Cable", ChildCategory = childCategories.First() },
                new Product { Name = "Keyboard", ChildCategory = childCategories.First() },
                new Product { Name = "Computer", ChildCategory = childCategories.First() },
                new Product { Name = "Monitor LG", ChildCategory = childCategories.Last() },
            });
            dbContext.SaveChanges();

            if (childCategoryId.HasValue && childCategoryId.Value == 1)
            {
                childCategoryId = childCategories.First().Id;
            }

            var products = productService.GetProductsFilter(searchString, childCategoryId);

            Assert.Equal(expected, products.Count());
        }

        [Theory]
        [InlineData("1", ProductsSort.Newest, "Monitor LG")]
        [InlineData("2", ProductsSort.Oldest, "USB Cable")]
        [InlineData("3", ProductsSort.PriceAscending, "Computer")]
        [InlineData("4", ProductsSort.PriceDescending, "Keyboard")]
        public void OrderByShouldOrderProducts(string test, ProductsSort productsSort, string productName)
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                        .UseInMemoryDatabase(databaseName: $"{test}_OrderBy_Product_Database")
                        .Options;

            var dbContext = new XeonDbContext(options);
            var productService = new ProductsService(dbContext);

            var parentCategory = new ParentCategory { Name = "Computers" };
            var childCategories = new List<ChildCategory>
            {
               new ChildCategory { Name = "Cables", ParentCategory = parentCategory},
               new ChildCategory { Name = "Monitors", ParentCategory = parentCategory }
            };
            dbContext.ChildCategories.AddRange(childCategories);
            dbContext.SaveChanges();

            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Name = "USB Cable", Price = 33, ChildCategory = childCategories.First() },
                new Product { Name = "Adapter Cable", Price = 28, ChildCategory = childCategories.First() },
                new Product { Name = "Keyboard", Price = 45, ChildCategory = childCategories.First() },
                new Product { Name = "Computer", Price = 11, ChildCategory = childCategories.First() },
                new Product { Name = "Monitor LG", Price = 36, ChildCategory = childCategories.Last() },
            });
            dbContext.SaveChanges();

            var products = productService.GetVisibleProducts();
            var orderedProducts = productService.OrderBy(products, productsSort);

            Assert.Equal(productName, orderedProducts.First().Name);
        }
    }
}