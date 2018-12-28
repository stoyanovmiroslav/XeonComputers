using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IAdressesService
    {
        Address CreateAddress(string street, string description, string city, string postcode);

        void AddAddressToUser(string username, Address address);

        IEnumerable<Address> GetAllUserAddresses(string name);
    }
}