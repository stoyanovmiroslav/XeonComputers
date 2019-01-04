using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.Services.Common;
using XeonComputers.Data;
using XeonComputers.Models;

namespace XeonComputers.Services
{
    public class ChildCategoriesService : IChildCategoriesService
    {
        private XeonDbContext db;

        public ChildCategoriesService(XeonDbContext db)
        {
            this.db = db;
        }

        public bool AddImageUrl(int id)
        {
            var category = this.db.ChildCategories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            category.ImageUrl = string.Format(GlobalConstants.CHILD_CATEGORY_PATH_TEMPLATE, id);
            this.db.SaveChanges();

            return true;
        }

        public ChildCategory CreateChildCategory(string name, string description, int parentId)
        {
            var childCategoty = new ChildCategory
            {
                Name = name,
                Description = description,
                ParentCategoryId = parentId
            };

            this.db.ChildCategories.Add(childCategoty);
            this.db.SaveChanges();

            return childCategoty;
        }

        public bool DeleteChildCategory(int id)
        {
            var category = this.db.ChildCategories.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            if (category == null || category.Products.Any())
            {
                return false;
            }

            this.db.ChildCategories.Remove(category);
            this.db.SaveChanges();

            return true;
        }

        public bool EditChildCategory(int id, string name, string description, int parentId)
        {
            var category = this.db.ChildCategories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            var isParentCategoryExist = this.db.ParentCategories.Any(x => x.Id == parentId);

            if (!isParentCategoryExist)
            {
                return false;
            }

            category.Name = name;
            category.Description = description;
            category.ParentCategoryId = parentId;

            this.db.SaveChanges();

            return true;
        }


        public IEnumerable<ChildCategory> GetChildCategories()
        {
            var categories = this.db.ChildCategories
                                    .Include(x => x.Products)
                                    .Include(x => x.ParentCategory)
                                    .ToArray();

            return categories;
        }

        public ChildCategory GetChildCategoryById(int id)
        {
            var category = this.db.ChildCategories.FirstOrDefault(x => x.Id == id);

            return category;
        }
    }
}
