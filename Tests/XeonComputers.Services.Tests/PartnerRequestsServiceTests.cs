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
    public class PartnerRequestsServiceTests
    {
        [Fact]
        public void CreateShouldCreatePartnerRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Create_PartnerRequest_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username };

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var partnerRequestService = new PartnerRequestsService(userService.Object, dbContext);

            partnerRequestService.Create(username);

            var partnerRequests = dbContext.PartnerRequests.ToList();

            Assert.Single(partnerRequests);
        }

        [Fact]
        public void CreateWhithInvalidUserShouldNotCreatePartnerRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateInvalidUser_PartnerRequest__Database")
                    .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            XeonUser user = null;

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var partnerRequestService = new PartnerRequestsService(userService.Object, dbContext);

            partnerRequestService.Create(username);

            var partnerRequests = dbContext.PartnerRequests.ToList();

            Assert.Empty(partnerRequests);
        }

        [Fact]
        public void CreateWhithTheSameUserRequestShouldNotCreateNewRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateTheSame_PartnerRequest_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username };

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var partnerRequestService = new PartnerRequestsService(userService.Object, dbContext);

            partnerRequestService.Create(username);
            partnerRequestService.Create(username);

            var partnerRequests = dbContext.PartnerRequests.ToList();

            Assert.Single(partnerRequests);
        }

        [Fact]
        public void RemoveShouldRemovePartnerRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "Remove_PartnerRequest_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username };

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var partnerRequestService = new PartnerRequestsService(userService.Object, dbContext);

            partnerRequestService.Create(username);

            var partnerRequest = dbContext.PartnerRequests.FirstOrDefault(x => x.XeonUser.UserName == username);
            partnerRequestService.Remove(partnerRequest.Id);

            var partnerRequests = dbContext.PartnerRequests.ToList();

            Assert.Empty(partnerRequests);
        }

        [Fact]
        public void RemoveWhithInvalidPartnerRequestIDShouldNotRemovePartnerRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "RemoveInvalid_PartnerRequest_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);

            var username = "user@gmail.com";
            var user = new XeonUser { UserName = username };

            var userService = new Mock<IUsersService>();
            userService.Setup(r => r.GetUserByUsername(username))
                       .Returns(user);

            var partnerRequestService = new PartnerRequestsService(userService.Object, dbContext);

            partnerRequestService.Create(username);

            var invalidPartnerRequestId = 123;
            partnerRequestService.Remove(invalidPartnerRequestId);

            var partnerRequests = dbContext.PartnerRequests.ToList();

            Assert.Single(partnerRequests);
        }

        [Fact]
        public void GetPartnersRequestsShouldReturnAllPartnerRequests()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetPartnersRequests_PartnerRequest_Database")
                    .Options;

            var dbContext = new XeonDbContext(options);
            var userService = new Mock<IUsersService>();
          

            var partnerRequests = new List<PartnerRequest>
            {
                new PartnerRequest{ XeonUser = new XeonUser { UserName = "user1@gmail.com" } },
                new PartnerRequest{ XeonUser = new XeonUser { UserName = "user2@gmail.com" } },
                new PartnerRequest{ XeonUser = new XeonUser { UserName = "user3@gmail.com" } },
            };

            dbContext.PartnerRequests.AddRange(partnerRequests);
            dbContext.SaveChanges();

            var partnerRequestService = new PartnerRequestsService(userService.Object, dbContext);
            var allPartnerRequests = partnerRequestService.GetPartnetsRequests();

            Assert.Equal(3, allPartnerRequests.Count());
        }
    }
}