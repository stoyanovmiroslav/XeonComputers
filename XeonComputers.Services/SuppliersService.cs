using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly XeonDbContext db;

        public SuppliersService(XeonDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Supplier> All()
        {
            return this.db.Suppliers.ToList();
        }

        public void Create(string name, decimal priceToHome, decimal priceToOffice)
        {
            var supplier = new Supplier
            {
                Name = name,
                PriceToHome = priceToHome,
                PriceToOffice = priceToOffice
            };

            if (!db.Suppliers.Any(x => x.IsDefault == true))
            {
                supplier.IsDefault = true;
            }

            this.db.Suppliers.Add(supplier);
            this.db.SaveChanges();
        }

        public bool MakeDafault(int id)
        {
            var newDefaultSupplier = this.db.Suppliers.FirstOrDefault(x => x.Id == id);

            if (newDefaultSupplier == null)
            {
                return false;
            }

            var oldDefaultSupplier = this.db.Suppliers.FirstOrDefault(x => x.IsDefault == true);
            if (oldDefaultSupplier != null)
            {
                oldDefaultSupplier.IsDefault = false;
            }

            newDefaultSupplier.IsDefault = true;
            this.db.SaveChanges();
            return true;
        }

        public decimal GetDiliveryPrice(int supplierId, DeliveryType deliveryType)
        {
            var supplier = this.db.Suppliers.FirstOrDefault(x => x.Id == supplierId);

            if (supplier == null)
            {
                return this.GetDefaultSupplier().PriceToHome;
            }

            if (deliveryType == DeliveryType.Home)
            {
                return supplier.PriceToHome;
            }
            else if (deliveryType == DeliveryType.Office)
            {
                return supplier.PriceToOffice;
            }

            return this.GetDefaultSupplier().PriceToHome;
        }

        public bool Delete(int id)
        {
            var supplier = this.db.Suppliers.FirstOrDefault(x => x.Id == id && x.IsDefault == false);

            if (supplier == null)
            {
                return false;
            }

            this.db.Suppliers.Remove(supplier);
            this.db.SaveChanges();

            return true;
        }

        public void Edit(int id, string name, decimal priceToHome, decimal priceToOffice)
        {
            var supplier = this.db.Suppliers.FirstOrDefault(x => x.Id == id);

            if (supplier == null)
            {
                return;
            }

            supplier.Name = name;
            supplier.PriceToHome = priceToHome;
            supplier.PriceToOffice = priceToOffice;

            this.db.SaveChanges();
        }

        public Supplier GetSupplierById(int id)
        {
            return this.db.Suppliers.FirstOrDefault(x => x.Id == id);
        }

        public Supplier GetDefaultSupplier()
        {
            return this.db.Suppliers.FirstOrDefault(x => x.IsDefault == true);
        }
    }
}