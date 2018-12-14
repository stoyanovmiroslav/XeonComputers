using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IShoppingCartService
    {
        void AddProductInShoppingCart(int productId, string username, int? quntity = null);

        void EditProductInShoppingCart(int productId, string username, int quantity);

        List<ShoppingCartProduct> GetAllShoppingCartProducts(string username);

        void DeleteProductFromShoppingCart(int id, string username);

        void DeleteAllProductFromsShoppingCart(string username);

        bool AnyProducts(string username);
    }
}