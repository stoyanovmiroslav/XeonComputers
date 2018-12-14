using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IProductsService
    {
        ICollection<Product> GetProducts();

        Product GetProductById(int id);

        ICollection<ChildCategory> GetChildCategories();

        void AddProduct(Product product);

        bool RemoveProduct(int id);

        bool ProductExists(int id);

        bool EditProduct(Product product);

        void AddImageUrls(int id, IList<string> imageUrls);

        IList<Image> GetImages(int id);

        IQueryable<Product> GetProductsQuery();

        IQueryable<Product> GetProductsByCategory(int childCategoryId);

        IList<Product> GetProductsBySearch(string searchString);
    }
}