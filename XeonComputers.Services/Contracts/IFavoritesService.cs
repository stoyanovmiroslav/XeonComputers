using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IFavoritesService
    {
        bool Add(int id, string name);

        IEnumerable<XeonUserFavoriteProduct> All(string name);

        void Delete(int id, string name);
    }
}