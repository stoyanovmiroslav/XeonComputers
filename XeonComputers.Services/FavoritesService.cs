using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly XeonDbContext db;

        public FavoritesService(XeonDbContext db)
        {
            this.db = db;
        }

        public bool Add(int id, string username)
        {
            var user = this.db.Users.Include(x => x.FavoriteProducts).FirstOrDefault(x => x.UserName == username);
            if (user == null || user.FavoriteProducts.Any(x => x.ProductId == id))
            {
                return false;
            }

            var isProductExist = this.db.Products.Any(x => x.Id == id);
            if (!isProductExist)
            {
                return false;
            }

            var xeonFavoritesProduct = new XeonUserFavoriteProduct
            {
                ProductId = id,
                XeonUserId = user.Id
            };

            user.FavoriteProducts.Add(xeonFavoritesProduct);
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<XeonUserFavoriteProduct> All(string username)
        {
            var favoriteProducts = this.db.XeonUserFavoriteProducts.Include(x => x.Product).ThenInclude(x => x.Images)
                                                       .Where(x => x.XeonUser.UserName == username);

            if (favoriteProducts == null)
            {
                return new List<XeonUserFavoriteProduct>();
            }

            return favoriteProducts;
        }

        public void Delete(int id, string username)
        {
            var favoriteProduct = this.db.XeonUserFavoriteProducts
                                         .FirstOrDefault(x => x.XeonUser.UserName == username && x.ProductId == id);

            if (favoriteProduct == null)
            {
                return;
            }

            this.db.XeonUserFavoriteProducts.Remove(favoriteProduct);
            this.db.SaveChanges();
        }
    }
}