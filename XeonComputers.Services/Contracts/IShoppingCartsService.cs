using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IShoppingCartsService
    {
        void AddProductInShoppingCart(int productId, string username, int? quntity = null);

        void EditProductQuantityInShoppingCart(int productId, string username, int quantity);

        IEnumerable<ShoppingCartProduct> GetAllShoppingCartProducts(string username);

        void DeleteProductFromShoppingCart(int id, string username);

        void DeleteAllProductFromShoppingCart(string username);

        bool AnyProducts(string username);
    }
}