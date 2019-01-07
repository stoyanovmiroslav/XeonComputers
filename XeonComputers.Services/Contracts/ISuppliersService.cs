using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;
using XeonComputers.Models.Enums;

namespace XeonComputers.Services.Contracts
{
    public interface ISuppliersService
    {
        void Create(string name, decimal priceToHome, decimal priceToOffice);

        IEnumerable<Supplier> All();

        bool MakeDafault(int id);

        decimal GetDiliveryPrice(int supplierId, DeliveryType deliveryType);

        bool Delete(int id);

        void Edit(int id, string name, decimal priceToHome, decimal priceToOffice);

        Supplier GetSupplierById(int id);

        Supplier GetDefaultSupplier();
    }
}