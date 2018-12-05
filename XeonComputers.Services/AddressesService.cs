using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class AddressesService : IAdressesService
    {
        private readonly IUsersService userService;
        private readonly XeonDbContext db;

        public AddressesService(IUsersService userService, XeonDbContext db)
        {
            this.userService = userService;
            this.db = db;
        }

        public void AddAddressesToUser(string username, Address address)
        {
            var user = this.userService.GetUserByUsername(username);

            user.Addresses.Add(address);

            this.db.SaveChanges();
        }

        public Address CreateAddress(string deliveryAddress, string additionТoАddress, string cityName, string postcode)
        {
            var city = this.GetOrCreateCity(cityName, postcode);

            var address = new Address
            {
                City = city,
                Street = deliveryAddress,
                Description = additionТoАddress
            };

            this.db.Addresses.Add(address);
            this.db.SaveChanges();

            return address;
        }

        public ICollection<Address> GetAllUserAddresses(string username)
        {
            return this.db.Addresses.Include(x => x.City).Where(x => x.XeonUser.UserName == username).ToList();
        }

        public City GetOrCreateCity(string cityName, string postcode)
        {
            var city = this.db.Cities.FirstOrDefault(x => x.Name == cityName && x.Postcode == postcode);

            if (city == null)
            {
                city = new City
                {
                    Name = cityName,
                    Postcode = postcode
                };

                this.db.Cities.Add(city);
                this.db.SaveChanges();
            }

            return city;
        }
    }
}