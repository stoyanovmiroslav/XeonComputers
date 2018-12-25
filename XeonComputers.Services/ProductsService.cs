using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Services
{
    public class ProductsService : IProductsService
    {
        private readonly XeonDbContext db;

        public ProductsService(XeonDbContext db)
        {
            this.db = db;
        }

        public void AddProduct(Product product)
        {
            this.db.Products.Add(product);
            this.db.SaveChanges();
        }

        public IEnumerable<ChildCategory> GetChildCategories()
        {
            return this.db.ChildCategories.ToArray();
        }

        public Product GetProductById(int id)
        {
            return this.db.Products.Include(p => p.ChildCategory)
                                   .Include(x => x.Images)
                                   .Include(x => x.Reviews)
                                   .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return db.Products.Include(p => p.ChildCategory)
                              .ThenInclude(x => x.ParentCategory)
                              .Include(x => x.Images)
                              .Include(x => x.Reviews)
                              .ToList();
        }

        public bool RemoveProduct(int id)
        {
            var product = this.db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            this.db.Products.Remove(product);
            this.db.SaveChanges();

            return true;
        }

        public bool ProductExists(int id)
        {
            return this.db.Products.Any(e => e.Id == id);
        }

        public bool EditProduct(Product product)
        {
            if (!this.ProductExists(product.Id))
            {
                return false;
            }

            try
            {
                this.db.Update(product);
                this.db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void AddImageUrls(int id, IEnumerable<string> imageUrls)
        {
            var product = this.GetProductById(id);

            foreach (var imageUrl in imageUrls)
            {
                var image = new Image { ImageUrl = imageUrl };
                product.Images.Add(image);
            }

            this.db.SaveChanges();
        }

        public IEnumerable<Image> GetImages(int id)
        {
            var product = GetProductById(id);

            return product.Images.ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int childCategoryId)
        {
            return db.Products.Where(x => x.ChildCategory.Id == childCategoryId)
                              .Include(p => p.ChildCategory)
                              .Include(x => x.Images)
                              .Include(x => x.Reviews);
        }

        public IEnumerable<Product> GetProductsBySearch(string searchString)
        {
            var searchStringClean = searchString.Split(new string[] { ",", ".", " " }, StringSplitOptions.RemoveEmptyEntries);

            IQueryable<Product> products = this.db.Products.Include(p => p.ChildCategory)
                                                           .ThenInclude(x => x.ParentCategory)
                                                           .Include(x => x.Images)
                                                           .Include(x => x.Reviews)
                                                           .Where(x => searchStringClean.All(c => x.Name.ToLower().Contains(c.ToLower())));
            return products;
        }

        public IEnumerable<Product> GetProductsFilter(string searchString, int? childCategoryId)
        {
            if (searchString != null)
            {
                return this.GetProductsBySearch(searchString);
            }
            else if (childCategoryId != null)
            {
                return this.GetProductsByCategory(childCategoryId.Value);
            }

            return this.GetProducts();
        }

        public IEnumerable<Product> OrderBy(IEnumerable<Product> products, ProductsSort sortBy)
        {
            if (ProductsSort.PriceDescending == sortBy)
            {
                return products.OrderByDescending(x => x.Price).ToList();
            }
            else if (ProductsSort.PriceAscending == sortBy)
            {
                return products.OrderBy(x => x.Price).ToList();
            }
            else if (ProductsSort.Oldest == sortBy)
            {
                //Default sorting behavior
                return products;
            }

            //ProductsSortType.Newest
            return products.OrderByDescending(x => x.Id).ToList();
        }

        public void AddRate(int rating, int productId)
        {
            var produc = this.GetProductById(productId);
            produc.Reviews.Add(new Review { Raiting = rating });

            this.db.SaveChanges();
        }
    }
}