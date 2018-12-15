using System;
using System.Collections.Generic;
using System.Text;
using XeonComputers.Models;

namespace XeonComputers.Services.Contracts
{
    public interface IAdressesService
    {
        Address CreateAddress(string deliveryAddress, string additionТoАddress, string city, string postcode);

        void AddAddressesToUser(string username, Address address);

        IEnumerable<Address> GetAllUserAddresses(string name);
    }
}