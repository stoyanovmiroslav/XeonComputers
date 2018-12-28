using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class AddressesServiceTests
    {
        [Fact]
        public void CreateAddressShouldCreateAddress()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Address_CreateAddress_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var addressService = new AddressesService(null, dbContext);

            addressService.CreateAddress("Street", "Description", "Sofiya", "2000");
            addressService.CreateAddress("Street", "Description", "Burgas", "8000");

            var addressCount = dbContext.Addresses.ToArray().Count();

            Assert.Equal(2, addressCount);
        }

        [Fact]
        public void GetOrCreateCityShouldReturnExistingCity()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Address_GetCity_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var addressService = new AddressesService(null, dbContext);

            var cityName = "Burgas";
            var cityPostcode = "8000";

            dbContext.Cities.Add(new City { Name = cityName, Postcode = cityPostcode });
            dbContext.SaveChanges();

            var city = addressService.GetOrCreateCity(cityName, cityPostcode);

            Assert.Equal(1, dbContext.Cities.Count());
            Assert.Equal(cityName, city.Name);
            Assert.Equal(cityPostcode, city.Postcode);
        }

        [Fact]
        public void GetOrCreateCityShouldCreateCity()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Address_CreateCity_Database")
                    .Options;
            var dbContext = new XeonDbContext(options);

            var addressService = new AddressesService(null, dbContext);

            var cityName = "Burgas";
            var cityPostcode = "8000";
            addressService.GetOrCreateCity(cityName, cityPostcode);

            var city = dbContext.Cities
                                .FirstOrDefault(x => x.Name == cityName &&
                                                     x.Postcode == cityPostcode);

            Assert.Equal(cityName, city.Name);
            Assert.Equal(cityPostcode, city.Postcode);
        }

        [Fact]
        public void AddAddressToUserShouldAddAddressDataCorrectly()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "Address_AddAddressToUser_Database")
                  .Options;
            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(dbContext.Users.FirstOrDefault(x => x.UserName == username));

            var addressService = new AddressesService(userService.Object, dbContext);

            var address = new Address
            {
                Street = "Ivan Vazov",
                Description = "106",
                BuildingNumber = "A",
                City = new City { Name = "Burgas", Postcode = "8000" },
                Country = "Bulgaria"
            };

            addressService.AddAddressToUser(username, address);

            var userAddress = dbContext.Users
                                       .FirstOrDefault(x => x.UserName == username)
                                       .Addresses
                                       .FirstOrDefault();

            Assert.Equal(address.Street, userAddress.Street);
            Assert.Equal(address.Description, userAddress.Description);
            Assert.Equal(address.BuildingNumber, userAddress.BuildingNumber);
            Assert.Equal(address.Country, userAddress.Country);
            Assert.Equal(address.City.Name, userAddress.City.Name);
            Assert.Equal(address.City.Postcode, userAddress.City.Postcode);
        }
    }
}