using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Controllers;
using BugTracker.Data;
using BugTracker.Data.Repositories;
using BugTracker.Models;
using BugTracker.Models.ViewModels.Issue;
using BugTracker.Services.ProducerNotificationService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BugTrackerTests
{
    public class IssueControllerTests
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfIssues()
        {
            // Arrange
            var controller = GetIssueController();
            var expectedModel = GetListOfIssueViewModel(GetTestIssues());

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, viewResult.ViewData.Model);
        }

        [Fact]
        public void CreateIssue_ReturnsAViewResult_WithAListOfUsersAndPriorities()
        {
            // Arrange
            var controller = GetIssueController();
            var expectedModel = GetCreateIssueViewModel();

            // Act
            var result = controller.CreateIssue();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, viewResult.ViewData.Model);
        }

        private static IssueController GetIssueController()
        {
            var mockLogger = new Mock<ILogger<IssueController>>();
            var mockUserRepo = new Mock<EfCoreRepository<User, ApplicationDbContext>>(null);
            mockUserRepo
                .Setup(userRepo => userRepo.GetAllObjects())
                .ReturnsAsync(GetTestUsers());

            var mockStatusRepo = new Mock<EfCoreRepository<Status, ApplicationDbContext>>(null);
            mockStatusRepo
                .Setup(statusRepo => statusRepo.GetObjectById(It.IsAny<int>()))
                .ReturnsAsync(GetTestStatuses().First);

            var mockPriorityRepo = new Mock<EfCoreRepository<Priority, ApplicationDbContext>>(null);
            mockPriorityRepo
                .Setup(priorityRepo => priorityRepo.GetAllObjects())
                .ReturnsAsync(GetTestPriorities());

            var userStoreMock = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var mockIRabbitMqProd = new Mock<IRabbitMqProducer<NotificationOfIssueAssignment>>();
            var mockIssueRepo = new Mock<EfCoreRepository<Issue, ApplicationDbContext>>(null);
            mockIssueRepo
                .Setup(issueRepo => issueRepo.GetAllObjects())
                .ReturnsAsync(GetTestIssues());
            mockIssueRepo
                .Setup(issueRepo => issueRepo.Create(It.IsAny<Issue>()))
                .ReturnsAsync(GetTestIssues().First);

            var controller = new IssueController(mockLogger.Object, mockIssueRepo.Object, mockUserRepo.Object,
                mockStatusRepo.Object, mockPriorityRepo.Object, mockUserManager.Object, mockIRabbitMqProd.Object);

            return controller;
        }

        private static IEnumerable<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new()
                {
                    Id = "bcef8a10-7bd9-4d36-811a-e322adfa4a97",
                    Login = "User#1"
                },
                new()
                {
                    Id = "dcef4a20-1899-4d36-511a-c322adca5a32",
                    Login = "User#2"
                }
            };

            return users;
        }

        private static IEnumerable<Priority> GetTestPriorities()
        {
            var priorities = new List<Priority>
            {
                new()
                {
                    Id = 1,
                    Name = "High"
                },
                new()
                {
                    Id = 2,
                    Name = "Low"
                }
            };

            return priorities;
        }

        private static IEnumerable<Status> GetTestStatuses()
        {
            var statuses = new List<Status>
            {
                new()
                {
                    Id = 1,
                    Name = "Opened"
                },
                new()
                {
                    Id = 2,
                    Name = "Closed"
                }
            };

            return statuses;
        }

        private static IEnumerable<Issue> GetTestIssues()
        {
            var priorities = GetTestPriorities().ToList();
            var statuses = GetTestStatuses().ToList();
            var users = GetTestUsers().ToList();

            var issues = new List<Issue>
            {
                new()
                {
                    Id = 1,
                    Title = "Title",
                    Description = "Description",
                    Created = DateTime.MinValue,
                    Updated = null,
                    CreatedBy = users.First(),
                    CreatedById = users.First().Id,
                    AssignedTo = null,
                    AssignedToId = null,
                    Priority = priorities.First(),
                    PriorityId = priorities.First().Id,
                    Status = statuses.First(),
                    StatusId = statuses.First().Id
                },
                new()
                {
                    Id = 2,
                    Title = "Title#2",
                    Description = "Description#2",
                    Created = DateTime.MinValue,
                    Updated = null,
                    CreatedBy = users.Last(),
                    CreatedById = users.Last().Id,
                    AssignedTo = null,
                    AssignedToId = null,
                    Priority = priorities.Last(),
                    PriorityId = priorities.Last().Id,
                    Status = statuses.Last(),
                    StatusId = statuses.Last().Id
                }
            };

            return issues;
        }

        private static IEnumerable<IssueViewModel> GetListOfIssueViewModel(IEnumerable<Issue> issues)
        {
            var issueListViewModel = issues.Select(i => new IssueViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Created = i.Created,
                Updated = i.Updated,
                CreatedBy = i.CreatedBy.Login,
                AssignedTo = i.AssignedTo?.Login,
                Priority = i.Priority.Name,
                Status = i.Status.Name
            }).ToList();

            return issueListViewModel;
        }
        
        private static CreateIssueViewModel GetCreateIssueViewModel()
        {
            var createIssueViewModel = new CreateIssueViewModel
            {
                Users = GetTestUsers(),
                Priorities = GetTestPriorities()
            };

            return createIssueViewModel;
        }

    }
    
}