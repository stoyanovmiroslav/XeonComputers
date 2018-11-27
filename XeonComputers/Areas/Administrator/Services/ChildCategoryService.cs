﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.Services.Contracts;
using XeonComputers.Common;
using XeonComputers.Data;
using XeonComputers.Models;

namespace XeonComputers.Areas.Administrator.Services
{
    public class ChildCategoryService : IChildCategoryService
    {
        private XeonDbContext db;

        public ChildCategoryService(XeonDbContext db)
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

            category.ImageUrl = string.Format(GlobalConstans.CHILD_CATEGORY_PATH_TEMPLATE, id);
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

            category.Name = name;
            category.Description = description;
            category.ParentCategoryId = parentId;

            this.db.SaveChanges();

            return true;
        }

        public ICollection<ChildCategory> GetChildCategories()
        {
            var categories = this.db.ChildCategories.Include(x => x.Products).Include(x => x.ParentCategory).ToArray();

            return categories;
        }

        public ChildCategory GetChildCategoryById(int id)
        {
            var category = this.db.ChildCategories.FirstOrDefault(x => x.Id == id);

            return category;
        }
    }
}