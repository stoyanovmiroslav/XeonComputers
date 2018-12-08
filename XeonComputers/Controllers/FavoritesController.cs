using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Favorites;

namespace XeonComputers.Controllers
{
    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        public IActionResult All()
        {
            IEnumerable<XeonUserFavoriteProduct> xeonUserFavoriteProducts = this.favoritesService.All(this.User.Identity.Name);

            var favoriteProductsViewModel = xeonUserFavoriteProducts.Select(x => new AllFavoriteViewModel
            {
                Id = x.ProductId,
                Name = x.Product.Name,
                ImageUrl = x.Product.Images.FirstOrDefault()?.ImageUrl,
                Price = x.Product.Price
            }).ToList();

            return View(favoriteProductsViewModel);
        }

        public IActionResult Add(int id)
        {
            this.favoritesService.Add(id, this.User.Identity.Name);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            this.favoritesService.Delete(id, this.User.Identity.Name);

            return RedirectToAction(nameof(All));
        }
    }
}