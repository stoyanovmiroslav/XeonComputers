using System.Collections.Generic;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IParentCategoriesService
    {
        void AddMainCategory(string name);

        ICollection<ParentCategory> GetParentCategories();

        ParentCategory GetParentCategoryById(int id);

        bool EditParentCategory(int id, string name);

        bool DeleteParentCategory(int id);
    }
}