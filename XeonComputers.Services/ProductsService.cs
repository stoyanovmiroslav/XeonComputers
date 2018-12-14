using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.Data;
using XeonComputers.Models;

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

        public ICollection<ChildCategory> GetChildCategories()
        {
            return this.db.ChildCategories.ToArray();
        }

        public Product GetProductById(int id)
        {
            return this.db.Products.Include(p => p.ChildCategory)
                                   .Include(x => x.Images)
                                   .FirstOrDefault(m => m.Id == id);
        }

        public ICollection<Product> GetProducts()
        {
            return db.Products.Include(p => p.ChildCategory)
                              .ThenInclude(x => x.ParentCategory)
                              .Include(x => x.Images)
                              .ToList();
        }

        public IQueryable<Product> GetProductsQuery()
        {
            return db.Products.Include(p => p.ChildCategory).Include(x => x.Images);
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

        public void AddImageUrls(int id, IList<string> imageUrls)
        {
            var product = this.GetProductById(id);

            foreach (var imageUrl in imageUrls)
            {
                var image = new Image { ImageUrl = imageUrl };
                product.Images.Add(image);
            }

            this.db.SaveChanges();
        }


        public IList<Image> GetImages(int id)
        {
            var product = GetProductById(id);

            return product.Images.ToList();
        }

        public IQueryable<Product> GetProductsByCategory(int childCategoryId)
        {
            return db.Products.Where(x => x.ChildCategory.Id == childCategoryId)
                       .Include(p => p.ChildCategory).Include(x => x.Images);
        }

        public IList<Product> GetProductsBySearch(string searchString)
        {
            var searchStringClean = searchString.Split(new string[] { ",", ".", " " }, StringSplitOptions.RemoveEmptyEntries);

            List<Product> products = this.db.Products.Include(p => p.ChildCategory)
                                                     .ThenInclude(x => x.ParentCategory)
                                                     .Include(x => x.Images)
                                                     .Where(x => searchStringClean.All(c => x.Name.ToLower().Contains(c.ToLower()))).ToList();
            return products;
        }
    }
}