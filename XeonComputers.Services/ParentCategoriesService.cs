using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Services.Contracts;
using XeonComputers.Data;
using XeonComputers.Models;

namespace XeonComputers.Services
{
    public class ParentCategoriesService : IParentCategoriesService
    {
        private XeonDbContext db;

        public ParentCategoriesService(XeonDbContext db)
        {
            this.db = db;
        }

        public ParentCategory CreateParentCategory(string name)
        {
            if (name == null)
            {
                return null;
            }

            var categoty = new ParentCategory { Name = name };

            this.db.ParentCategories.Add(categoty);
            this.db.SaveChanges();

            return categoty;
        }

        public bool DeleteParentCategory(int id)
        {
            var category = this.db.ParentCategories
                                  .Include(x => x.ChildCategories)
                                  .FirstOrDefault(x => x.Id == id);

            if (category == null || category.ChildCategories.Count != 0)
            {
                return false;
            }

            this.db.ParentCategories.Remove(category);
            this.db.SaveChanges();

            return true;
        }

        public bool EditParentCategory(int id, string name)
        {
            var category = this.db.ParentCategories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            category.Name = name;
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<ParentCategory> GetParentCategories()
        {
            var categories = this.db.ParentCategories.Include(x => x.ChildCategories).ToArray();

            return categories;
        }

        public ParentCategory GetParentCategoryById(int id)
        {
            var category = this.db.ParentCategories.FirstOrDefault(x => x.Id == id);

            return category;
        }
    }
}
