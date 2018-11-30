using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.Services.Contracts
{
    public interface IProductService
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
    }
}