using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Common;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class ChildCategoriesServiceTests
    {
        [Fact]
        public void GetChildCategoriesShouldReturnAllChildCategories()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetChildCategories_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategory = new ParentCategory { Name = "Computers" };

            dbContext.ChildCategories.AddRange(new List<ChildCategory>
            {
                new ChildCategory { Name = "Cables", ParentCategoryId = parentCategory.Id, ParentCategory = parentCategory },
                new ChildCategory { Name = "Monitors", ParentCategoryId = parentCategory.Id, ParentCategory = parentCategory }
            });
            dbContext.SaveChanges();

            var childCategoriesService = new ChildCategoriesService(dbContext);
            var childCategories = childCategoriesService.GetChildCategories();

            Assert.Equal(2, childCategories.Count());
        }

        [Fact]
        public void GetChildCategoryByIdShouldReturnChildCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetChildCategoryById_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategory = new ParentCategory { Name = "Computers" };

            dbContext.ChildCategories.AddRange(new List<ChildCategory>
            {
                new ChildCategory { Id = 1, Name = "Cables", ParentCategory = parentCategory },
                new ChildCategory { Id = 2, Name = "Monitors", ParentCategory = parentCategory }
            });
            dbContext.SaveChanges();

            var childCategoriesService = new ChildCategoriesService(dbContext);
            var childCategory = childCategoriesService.GetChildCategoryById(1);

            Assert.Equal("Cables", childCategory.Name);
        }

        [Fact]
        public void CreateChildCategoryShouldCreateChildCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateChildCategory_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            dbContext.ParentCategories.Add(new ParentCategory { Name = "Computers" });
            dbContext.SaveChanges();

            var parentCategoryName = "Computers";
            var parentCategory = dbContext.ParentCategories.FirstOrDefault(x => x.Name == parentCategoryName);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var childCategoryName = "Cables";
            var childCategory = childCategoriesService.CreateChildCategory(childCategoryName, null, parentCategory.Id);

            Assert.Equal(1, dbContext.ChildCategories.Count());
            Assert.Equal(childCategoryName, childCategory.Name);
        }

        [Fact]
        public void AddImageUrlShouldReturnTrueAndAddImageUrl()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "AddImageUrly_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            dbContext.ParentCategories.Add(new ParentCategory { Name = "Computers" });
            dbContext.SaveChanges();

            var parentCategoryName = "Computers";
            var parentCategory = dbContext.ParentCategories.FirstOrDefault(x => x.Name == parentCategoryName);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var childCategoryName = "Cables";
            var childCategory = childCategoriesService.CreateChildCategory(childCategoryName, null, parentCategory.Id);

            var isImageUrlCreated = childCategoriesService.AddImageUrl(childCategory.Id);

            var childCategoryExpectedImageUrl = string.Format(GlobalConstants.CHILD_CATEGORY_PATH_TEMPLATE, childCategory.Id);

            Assert.Equal(childCategoryExpectedImageUrl, childCategory.ImageUrl);
            Assert.True(isImageUrlCreated);
        }

        [Fact]
        public void AddImageUrlShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "AddImageUrly_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var invalidChildCategoryId = 120;
            var isImageUrlCreated = childCategoriesService.AddImageUrl(invalidChildCategoryId);

            Assert.False(isImageUrlCreated);
        }

        [Fact]
        public void EditChildCategoryShouldReturnTrueAndEditCorrectlyChildCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "EditChildCategory_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryName = "Computers";
            dbContext.ParentCategories.Add(new ParentCategory { Name = parentCategoryName });
            dbContext.SaveChanges();

            var parentCategory = dbContext.ParentCategories.FirstOrDefault(x => x.Name == parentCategoryName);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var childCategoryName = "Cables";
            var childCategoryDescription = "USB";
            var childCategory = childCategoriesService.CreateChildCategory(childCategoryName, childCategoryDescription, parentCategory.Id);

            var newChildCategoryName = "Monitors";
            var newChildCategoryDescription = "17''";
            var isChildCategoryEdit = childCategoriesService.EditChildCategory(childCategory.Id, newChildCategoryName, newChildCategoryDescription, parentCategory.Id);

            var childCategoryExpectedImageUrl = string.Format(GlobalConstants.CHILD_CATEGORY_PATH_TEMPLATE, childCategory.Id);

            Assert.Equal(newChildCategoryName, childCategory.Name);
            Assert.Equal(newChildCategoryDescription, childCategory.Description);
            Assert.True(isChildCategoryEdit);
        }

        [Fact]
        public void EditChildCategoryWhithInvalidChildCategoryIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "EditChildCategory_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var newChildCategoryName = "Monitors";
            var newChildCategoryDescription = "17''";
            var invalidChildCategoryId = 121;
            var parentCategoryId = 131;
            var isChildCategoryEdit = childCategoriesService.EditChildCategory(invalidChildCategoryId, newChildCategoryName, newChildCategoryDescription, parentCategoryId);

            Assert.False(isChildCategoryEdit);
        }

        [Fact]
        public void EditChildCategoryWhithInvaliParentCategoryIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "EditChildCategory_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryName = "Computers";
            dbContext.ParentCategories.Add(new ParentCategory { Name = parentCategoryName });
            dbContext.SaveChanges();

            var parentCategory = dbContext.ParentCategories.FirstOrDefault(x => x.Name == parentCategoryName);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var childCategoryName = "Cables";
            var childCategoryDescription = "USB";
            var childCategory = childCategoriesService.CreateChildCategory(childCategoryName, childCategoryDescription, parentCategory.Id);

            var newChildCategoryName = "Monitors";
            var newChildCategoryDescription = "17''";
            var invalidParentCategoryId = 121;
            var isChildCategoryEdit = childCategoriesService.EditChildCategory(childCategory.Id, newChildCategoryName, newChildCategoryDescription, invalidParentCategoryId);

            Assert.False(isChildCategoryEdit);
        }

        [Fact]
        public void DeleteChildCategoryShouldReturnTrueAndDeleteChildCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteChildCategoryCorrect_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryName = "Computers";
            dbContext.ParentCategories.Add(new ParentCategory { Name = parentCategoryName });
            dbContext.SaveChanges();

            var parentCategory = dbContext.ParentCategories.FirstOrDefault(x => x.Name == parentCategoryName);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var childCategoryName = "Cables";
            var childCategoryDescription = "USB";
            var childCategory = childCategoriesService.CreateChildCategory(childCategoryName, childCategoryDescription, parentCategory.Id);

            var isChildCategoryDeleted = childCategoriesService.DeleteChildCategory(childCategory.Id);

            Assert.Equal(0, dbContext.ChildCategories.Count());
            Assert.True(isChildCategoryDeleted);
        }

        [Fact]
        public void DeleteChildCategoryWithInvalidChildCategoryIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteChildCategory_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var invalidChildCategory = 132;
            var isChildCategoryDeleted = childCategoriesService.DeleteChildCategory(invalidChildCategory);

            Assert.False(isChildCategoryDeleted);
        }

        [Fact]
        public void DeleteChildCategoryWithProductsShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteChildCategory_ChildCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryName = "Computers";
            dbContext.ParentCategories.Add(new ParentCategory { Name = parentCategoryName });
            dbContext.SaveChanges();

            var parentCategory = dbContext.ParentCategories.FirstOrDefault(x => x.Name == parentCategoryName);

            var childCategoriesService = new ChildCategoriesService(dbContext);

            var childCategoryName = "Cables";
            var childCategoryDescription = "USB";
            var childCategory = childCategoriesService.CreateChildCategory(childCategoryName, childCategoryDescription, parentCategory.Id);

            childCategory.Products = new List<Product> { new Product { Name = "Headsets" } };
            dbContext.SaveChanges();

            var isChildCategoryDeleted = childCategoriesService.DeleteChildCategory(childCategory.Id);

            Assert.False(isChildCategoryDeleted);
            Assert.Equal(1, dbContext.ChildCategories.Count());
        }
    }
}