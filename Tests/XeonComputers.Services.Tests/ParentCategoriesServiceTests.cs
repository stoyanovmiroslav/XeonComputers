using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class ParentCategoriesServiceTests
    {
        [Fact]
        public void CreateParentCategoryShouldCreateMainCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateParentCategory_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            parentCategoryService.CreateParentCategory("Computers");
            parentCategoryService.CreateParentCategory("Phones");

            Assert.Equal(2, dbContext.ParentCategories.Count());
        }

        [Fact]
        public void CreateParentCategoryWhithNullParameterShouldNotCreateMainCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateParentCategory_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            parentCategoryService.CreateParentCategory(null);

            Assert.Equal(0, dbContext.ParentCategories.Count());
        }

        [Fact]
        public void GetParentCategoriesShouldReturnAllParentCategories()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetParentCategories_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            parentCategoryService.CreateParentCategory("Computers");
            parentCategoryService.CreateParentCategory("Phones");

            var parentCategories = parentCategoryService.GetParentCategories();

            Assert.Equal(2, parentCategories.Count());
        }

        [Fact]
        public void GetParentCategoryByIdShouldReturnParentCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetParentCategoryById_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            var parentCategoryComputers = parentCategoryService.CreateParentCategory("Computers");
            var parentCategoryPhones = parentCategoryService.CreateParentCategory("Phones");

            var parentCategory = parentCategoryService.GetParentCategoryById(parentCategoryPhones.Id);

            Assert.Equal(parentCategoryPhones.Id, parentCategory.Id);
            Assert.Equal(parentCategoryPhones.Name, parentCategory.Name);
        }

        [Fact]
        public void EditParentCategoryByIdShouldReturnTrueAndEditParentCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "EditParentCategory_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            var parentCategoryComputers = parentCategoryService.CreateParentCategory("Computers");
            var parentCategoryPhones = parentCategoryService.CreateParentCategory("Phones");

            var newParentCategoryName = "NewComputers";
            var isParentCategoryEdit = parentCategoryService.EditParentCategory(parentCategoryComputers.Id, newParentCategoryName);

            Assert.Equal(newParentCategoryName, parentCategoryComputers.Name);
            Assert.True(isParentCategoryEdit);
        }

        [Fact]
        public void EditParentCategoryByIdWhithInvalidParentCategoryIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "EditParentCategory_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            var invalidParentCategoryId = 123;
            var newParentCategoryName = "Computers";
            var isParentCategoryEdit = parentCategoryService.EditParentCategory(invalidParentCategoryId, newParentCategoryName);

            Assert.False(isParentCategoryEdit);
        }

        [Fact]
        public void DeleteParentCategoryShouldReturnTrueAndDeleteParentCategory()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteParentCategoryCorrect_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            var parentCategory = parentCategoryService.CreateParentCategory("Computers");

            var isParentCategoryDeleted = parentCategoryService.DeleteParentCategory(parentCategory.Id);

            Assert.Equal(0, dbContext.ParentCategories.Count());
            Assert.True(isParentCategoryDeleted);
        }

        [Fact]
        public void DeleteParentCategoryWhithInvalidParentCategoryIdShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteParentCategoryWhithInvalidParentCategoryId_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            var parentCategory = parentCategoryService.CreateParentCategory("Computers");

            var invalidParentCategoryId = 123;
            var isParentCategoryDeleted = parentCategoryService.DeleteParentCategory(invalidParentCategoryId);

            Assert.Equal(1, dbContext.ParentCategories.Count());
            Assert.False(isParentCategoryDeleted);
        }

        [Fact]
        public void DeleteParentCategoryWhithChildCategoriesShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteParentCategory_ParentCategories_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var parentCategoryService = new ParentCategoriesService(dbContext);

            var parentCategory = parentCategoryService.CreateParentCategory("Computers");
            parentCategory.ChildCategories = new List<ChildCategory> { new ChildCategory { Name = "Cabels" } };
            dbContext.SaveChanges();

            var invalidParentCategoryId = 123;
            var isParentCategoryDeleted = parentCategoryService.DeleteParentCategory(invalidParentCategoryId);

            Assert.Equal(1, dbContext.ParentCategories.Count());
            Assert.False(isParentCategoryDeleted);
        }
    }
}
