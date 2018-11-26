using System.Collections.Generic;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.Services.Contracts
{
    public interface IParentCategoryService
    {
        void AddMainCategory(string name);

        ICollection<ParentCategory> GetParentCategories();

        ParentCategory GetParentCategoryById(int id);

        bool EditParentCategory(int id, string name);

        bool DeleteParentCategory(int id);
    }
}