using System.Collections.Generic;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IParentCategoriesService
    {
        ParentCategory CreateParentCategory(string name);

        IEnumerable<ParentCategory> GetParentCategories();

        ParentCategory GetParentCategoryById(int id);

        bool EditParentCategory(int id, string name);

        bool DeleteParentCategory(int id);
    }
}