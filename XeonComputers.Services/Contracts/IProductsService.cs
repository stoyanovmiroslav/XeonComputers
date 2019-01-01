﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Services.Contracts
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();

        Product GetProductById(int id);

        void AddProduct(Product product);

        bool RemoveProduct(int id);

        bool ProductExists(int id);

        bool EditProduct(Product product);

        void AddImageUrls(int id, IEnumerable<string> imageUrls);

        IEnumerable<Image> GetImages(int id);

        IEnumerable<Product> GetProductsByCategory(int childCategoryId);

        IEnumerable<Product> GetProductsBySearch(string searchString);

        IEnumerable<Product> GetProductsFilter(string searchString, int? childCategoryId);

        IEnumerable<Product> OrderBy(IEnumerable<Product> products, ProductsSort sortBy);

        void AddReview(int rating, int productId);
    }
}