using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using XeonComputers.Data;
using XeonComputers.Models;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;
using Xunit;

namespace XeonComputers.Services.Tests
{
    public class UserRequestsServiceTests
    {
        [Fact]
        public void CreateShouldCreateUserRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Create_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            var title = "Question";
            var email = "user@gmail.com";
            var content = "content";
            userRequestsService.Create(title, email, content);

            var userRequests = dbContext.UserRequests.ToList();

            Assert.Single(userRequests);
            Assert.Equal(title, userRequests.First().Title);
            Assert.Equal(email, userRequests.First().Email);
            Assert.Equal(content, userRequests.First().Content);
            Assert.Equal(DateTime.UtcNow.AddHours(GlobalConstants.BULGARIAN_HOURS_FROM_UTC_TIME).Hour,
                userRequests.First().RequestDate.Hour);
        }

        [Fact]
        public void AllShouldReturnAllUserRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "All_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            dbContext.UserRequests.AddRange(new List<UserRequest>
            {
                new UserRequest { Title = "Question", Content = "content" },
                new UserRequest { Title = "Request", Content = "content1" }
            });
            dbContext.SaveChanges();

            var userRequests = userRequestsService.All();

            Assert.Equal(2, userRequests.Count());
        }

        [Fact]
        public void GetRequestByIdShouldReturnUserRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "GetRequestById_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            var userRequestId = 1;
            var userRequestTitle = "Request-1";
            dbContext.UserRequests.AddRange(new List<UserRequest>
            {
                new UserRequest { Id = userRequestId, Title = userRequestTitle },
                new UserRequest { Id = 2, Title = "Request-2" },
                new UserRequest { Id = 3, Title = "Request-3" },
            });
            dbContext.SaveChanges();

            var userRequest = userRequestsService.GetRequestById(userRequestId);

            Assert.Equal(userRequestTitle, userRequest.Title);
            Assert.Equal(userRequestId, userRequest.Id);
        }

        [Fact]
        public void SeenShouldChangeIsSeenOnTrue()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Seen_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            var userRequestId = 1;
            var userRequestTitle = "Request-1";
            dbContext.UserRequests.AddRange(new List<UserRequest>
            {
                new UserRequest { Id = userRequestId, Title = userRequestTitle },
                new UserRequest { Id = 2, Title = "Request-2" },
                new UserRequest { Id = 3, Title = "Request-3" },
            });
            dbContext.SaveChanges();

            userRequestsService.Seen(userRequestId);

            var userRequest = dbContext.UserRequests.FirstOrDefault(x => x.Id == userRequestId);

            Assert.True(userRequest.Seen);
        }

        [Fact]
        public void UnseenShouldChangeIsSeenOnFalse()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Unseen_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            var userRequestId = 1;
            var userRequestTitle = "Request-1";
            dbContext.UserRequests.AddRange(new List<UserRequest>
            {
                new UserRequest { Id = userRequestId, Title = userRequestTitle, Seen = true },
                new UserRequest { Id = 2, Title = "Request-2" },
                new UserRequest { Id = 3, Title = "Request-3" },
            });
            dbContext.SaveChanges();

            userRequestsService.Unseen(userRequestId);

            var userRequest = dbContext.UserRequests.FirstOrDefault(x => x.Id == userRequestId);

            Assert.False(userRequest.Seen);
        }

        [Fact]
        public void GetUnseenRequestsShouldReturneAllGetUnSeenRequests()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "GetUnseenRequests_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            var userRequestId = 1;
            var userRequestTitle = "Request-1";
            dbContext.UserRequests.AddRange(new List<UserRequest>
            {
                new UserRequest { Id = userRequestId, Title = userRequestTitle, Seen = true },
                new UserRequest { Id = 2, Title = "Request-2" },
                new UserRequest { Id = 3, Title = "Request-3" },
            });
            dbContext.SaveChanges();

            var unseenRequests = userRequestsService.GetUnseenRequests();

            Assert.Equal(2, unseenRequests.Count());
        }

        [Fact]
        public void DeleteShouldReturnTrueAndDeleteUserRequest()
        {
            var options = new DbContextOptionsBuilder<XeonDbContext>()
             .UseInMemoryDatabase(databaseName: "Delete_UserRequests_Database")
             .Options;
            var dbContext = new XeonDbContext(options);

            var userRequestsService = new UserRequestsService(dbContext);

            var userRequestId = 1;
            var userRequestTitle = "Request-1";
            dbContext.UserRequests.AddRange(new List<UserRequest>
            {
                new UserRequest { Id = userRequestId, Title = userRequestTitle, Seen = true },
                new UserRequest { Id = 2, Title = "Request-2" },
                new UserRequest { Id = 3, Title = "Request-3" },
            });
            dbContext.SaveChanges();

            var isDeleted = userRequestsService.Delete(userRequestId);

            var userRequest = dbContext.UserRequests.FirstOrDefault(x => x.Id == userRequestId);

            Assert.Null(userRequest);
            Assert.True(isDeleted);
        }
    }
}