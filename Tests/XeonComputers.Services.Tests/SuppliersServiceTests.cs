using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class SuppliersServiceTests
    {
        [Fact]
        public void CreateShouldCreateSupplierAndMakeDefaultTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Create_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var name = "DHL";
            var priceToHome = 5.5M;
            var priceToOffice = 4.5M;
            suppliersService.Create(name, priceToHome, priceToOffice);

            var supplier = dbContext.Suppliers.FirstOrDefault(x => x.Name == name);

            Assert.NotNull(supplier);
            Assert.Equal(name, supplier.Name);
            Assert.Equal(priceToHome, supplier.PriceToHome);
            Assert.Equal(priceToOffice, supplier.PriceToOffice);
            Assert.True(supplier.IsDefault);
        }

        [Fact]
        public void CreateShouldCreateSupplierAndIsDefaultShouldStayFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "CreateIsDefaultFalse_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            dbContext.Suppliers.Add(new Supplier { Name = "Econt", IsDefault = true });
            dbContext.SaveChanges();

            var name = "DHL";
            var priceToHome = 5.5M;
            var priceToOffice = 4.5M;
            suppliersService.Create(name, priceToHome, priceToOffice);

            var supplier = dbContext.Suppliers.FirstOrDefault(x => x.Name == name);

            Assert.NotNull(supplier);
            Assert.Equal(name, supplier.Name);
            Assert.Equal(priceToHome, supplier.PriceToHome);
            Assert.Equal(priceToOffice, supplier.PriceToOffice);
            Assert.False(supplier.IsDefault);
        }

        [Fact]
        public void MakeDafaultShouldChangeIsDefaultToTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "MakeDafault_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var supplier = new Supplier
            {
                Name = "Econt",
                IsDefault = false
            };
            dbContext.Suppliers.Add(supplier);
            dbContext.SaveChanges();

            suppliersService.MakeDafault(supplier.Id);

            Assert.True(supplier.IsDefault);
        }

        [Fact]
        public void MakeDafaultShouldSwapDefaultSupplier()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "MakeDafaultSwap_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var suppliers = new List<Supplier>
            {
                new Supplier{ Name = "Econt", IsDefault = true },
                new Supplier{ Name = "DHL", IsDefault = false },
            };
            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();

            suppliersService.MakeDafault(suppliers.Last().Id);

            Assert.False(suppliers.First().IsDefault);
            Assert.True(suppliers.Last().IsDefault);
        }

        [Fact]
        public void DeleteShouldDeleteSupplier()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Delete_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var suppliers = new List<Supplier>
            {
                new Supplier{ Name = "Econt", IsDefault = true },
                new Supplier{ Name = "DHL", IsDefault = false },
            };
            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();

            var isDeleted = suppliersService.Delete(suppliers.Last().Id);

            Assert.Single(dbContext.Suppliers);
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteDefaultSupplierShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "DeleteDefaultSupplier_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var suppliers = new List<Supplier>
            {
                new Supplier{ Name = "Econt", IsDefault = true },
                new Supplier{ Name = "DHL", IsDefault = false },
            };
            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();

            var isDeleted = suppliersService.Delete(suppliers.First().Id);

            Assert.Equal(2, dbContext.Suppliers.Count());
            Assert.False(isDeleted);
        }

        [Fact]
        public void GetSupplierByIdShouldReturnSupplier()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "GetSupplierById_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var supplierId = 1;
            var supplierName = "Econt";
            var suppliers = new List<Supplier>
            {
                new Supplier{ Id = supplierId, Name = supplierName, IsDefault = true },
                new Supplier{ Id = 2, Name = "DHL", IsDefault = false },
            };
            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();

            var supplier = suppliersService.GetSupplierById(supplierId);
            Assert.Equal(supplierName, supplier.Name);
        }

        [Fact]
        public void EditShouldEditSupplier()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Edit_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var supplier = new Supplier
            {
                Name = "DHL",
                PriceToHome = 4.5M,
                PriceToOffice = 5.5M
            };
            dbContext.Suppliers.Add(supplier);
            dbContext.SaveChanges();

            var name = "econt";
            var priceToHome = 3.5M;
            var priceToOffice = 3M;
            suppliersService.Edit(supplier.Id, name, priceToHome, priceToOffice);

            Assert.Equal(name, supplier.Name);
            Assert.Equal(priceToHome, supplier.PriceToHome);
            Assert.Equal(priceToOffice, supplier.PriceToOffice);
        }

        [Fact]
        public void AllShouldReturnAllSuppliers()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "All_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var suppliers = new List<Supplier>
            {
                new Supplier{ Name = "Econt", IsDefault = true },
                new Supplier{ Name = "DHL", IsDefault = false },
            };
            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();

            Assert.Equal(2, suppliersService.All().Count());
        }

        [Fact]
        public void GetDiliveryPriceShouldReturnDiliveryPrice()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "GetDiliveryPrice_Supplier_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var suppliersService = new SuppliersService(dbContext);

            var suppliers = new List<Supplier>
            {
                new Supplier{ Name = "Econt", PriceToHome = 4.5M, PriceToOffice = 3.5M },
                new Supplier{ Name = "DHL", PriceToHome = 3.2M, PriceToOffice = 2.8M },
            };
            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();

            var homeDeliveryPrice = suppliersService.GetDiliveryPrice(suppliers.First().Id, DeliveryType.Home);
            var officeDeliveryPrice = suppliersService.GetDiliveryPrice(suppliers.First().Id, DeliveryType.Office);

            Assert.Equal(homeDeliveryPrice, suppliers.First().PriceToHome);
            Assert.Equal(officeDeliveryPrice, suppliers.First().PriceToOffice);
        }
    }
}
