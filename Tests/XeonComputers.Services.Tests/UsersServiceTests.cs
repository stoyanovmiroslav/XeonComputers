using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class UsersServiceTests
    {
        [Fact]
        public void GetUserCompanyByUsernameShouldReturnCurrentUserCompany()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "GetUserCompanyByUsername_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var mockUserStore = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var usersService = new UsersService(dbContext, userManager.Object);

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                Company = new Company { Name = "Computers Ltd" }
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var company = usersService.GetUserCompanyByUsername(user.UserName);

            Assert.Equal(user.Company.Name, company.Name);
        }

        [Fact]
        public void CreateCompanyShouldCreateUserCompanyAndReturTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "CreateCompany_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var mockUserStore = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(user.UserName))
                        .Returns(Task.FromResult<XeonUser>(user));

            var usersService = new UsersService(dbContext, userManager.Object);


            var company = new Company { Name = "Computers Ltd" };
            var isCreated = usersService.CreateCompany(company, user.UserName);

            Assert.True(isCreated);
            Assert.Equal(company.Name, user.Company.Name);
        }

        [Fact]
        public void CreateCompanyWithInvalidUserShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "CreateCompanyFalse_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            XeonUser user = null;

            var mockUserStore = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                        .Returns(Task.FromResult<XeonUser>(user));

            var usersService = new UsersService(dbContext, userManager.Object);

            var company = new Company { Name = "Computers Ltd" };
            var isCreated = usersService.CreateCompany(company, username);

            Assert.False(isCreated);
        }

        [Fact]
        public void GetUsersWithPartnersRequstsShouldReturnAllUsersWithPartnersRequst()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "GetUsersWithPartnersRequsts_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            dbContext.Users.AddRange(new List<XeonUser>
            {
                new XeonUser { UserName = "user1", PartnerRequest = new PartnerRequest() },
                new XeonUser { UserName = "user2", PartnerRequest = new PartnerRequest() },
                new XeonUser { UserName = "user3", },
                new XeonUser { UserName = "user4", }
            });
            dbContext.SaveChanges();

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);

            var usersService = new UsersService(dbContext, userManager.Object);

            var users = usersService.GetUsersWithPartnersRequsts();

            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void GetUsersByRoleShouldReturnAllUsersInRole()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "GetUsersByRole_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var userInRoleAdmin = "user3";
            var users = new List<XeonUser>
            {
                new XeonUser { UserName = "user1", },
                new XeonUser { UserName = "user2", },
                new XeonUser { UserName = userInRoleAdmin, },
                new XeonUser { UserName = "user4", },
                new XeonUser { UserName = "user5", }
            };
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();

            var role = "Admin";
            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetUsersInRoleAsync(role))
                       .Returns(Task.FromResult<IList<XeonUser>>(users.Skip(2).Take(1).ToList()));

            var usersService = new UsersService(dbContext, userManager.Object);

            var usersInRole = usersService.GetUsersByRole(role);

            Assert.Single(usersInRole);
            Assert.Equal(userInRoleAdmin, usersInRole.First().UserName);
        }

        [Fact]
        public void AddUserToRoleShouldAddUserToRoleAndReturnTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "AddUserToRole_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user3";
            var users = new List<XeonUser>
            {
                new XeonUser { UserName = "user1", },
                new XeonUser { UserName = "user2", },
                new XeonUser { UserName = username, }
            };
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                       .Returns(Task.FromResult<XeonUser>(users.FirstOrDefault(x => x.UserName == username)));

            var usersService = new UsersService(dbContext, userManager.Object);

            var role = "Admin";
            var isUserAddInRole = usersService.AddUserToRole(username, role);

            Assert.True(isUserAddInRole);
        }

        [Fact]
        public void AddUserToRoleWhithInvalidUserShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "AddUserToRoleFalse_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user3";
            XeonUser user = null;

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                       .Returns(Task.FromResult<XeonUser>(user));

            var usersService = new UsersService(dbContext, userManager.Object);

            var role = "Admin";
            var isUserAddInRole = usersService.AddUserToRole(username, role);

            Assert.False(isUserAddInRole);
        }

        [Fact]
        public void RemoveUserFromToRoleWithInvalidUserShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "RemoveUserFromToRoleFalse_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user3";
            XeonUser user = null;

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                       .Returns(Task.FromResult<XeonUser>(user));

            var usersService = new UsersService(dbContext, userManager.Object);

            var role = "Admin";
            var isUserRomeveFromRole = usersService.RemoveUserFromToRole(username, role);

            Assert.False(isUserRomeveFromRole);
        }

        [Fact]
        public void RemoveUserFromToRoleShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "RemoveUserFromToRole_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
            };
            dbContext.Users.Add(user);

            var roleName = "Admin";
            var role = new IdentityRole { Name = roleName };
            dbContext.Roles.Add(role);
            dbContext.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
            dbContext.SaveChanges();

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(user.UserName))
                       .Returns(Task.FromResult<XeonUser>(user));

            var usersService = new UsersService(dbContext, userManager.Object);
            var isUserRomeveFromRole = usersService.RemoveUserFromToRole(user.UserName, roleName);

            Assert.True(isUserRomeveFromRole);
        }

        [Fact]
        public void EditFirstNameRequstsShouldEditFirstName()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "EditFirstName_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                FirstName = "AdminFirstName"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);

            var usersService = new UsersService(dbContext, userManager.Object);

            var firstName = "UserFirstName";
            usersService.EditFirstName(user, firstName);

            Assert.Equal(firstName, user.FirstName);
        }

        [Fact]
        public void EditLastNameRequstsShouldEditLastName()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                  .UseInMemoryDatabase(databaseName: "EditLastName_Users_Database")
                  .Options;

            var dbContext = new XeonDbContext(options);

            var user = new XeonUser
            {
                UserName = "user@gmail.com",
                LastName = "AdminLastName"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var store = new Mock<IUserStore<XeonUser>>();
            var userManager = new Mock<UserManager<XeonUser>>(store.Object, null, null, null, null, null, null, null, null);

            var usersService = new UsersService(dbContext, userManager.Object);

            var lastName = "UserLastName";
            usersService.EditLastName(user, lastName);

            Assert.Equal(lastName, user.LastName);
        }
    }
}