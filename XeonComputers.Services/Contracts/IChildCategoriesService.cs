using System.Collections.Generic;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IChildCategoriesService
    {
        IEnumerable<ChildCategory> GetChildCategories();

        ChildCategory GetChildCategoryById(int id);

        ChildCategory CreateChildCategory(string name, string description, int parentId);

        bool AddImageUrl(int id);

        bool EditChildCategory(int id, string name, string description, int parentId);

        bool DeleteChildCategory(int id);
    }
}