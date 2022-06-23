using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Controllers;
using BugTracker.Models;
using BugTracker.Models.Domain.Vacancy;
using BugTracker.Models.ViewModels;
using BugTracker.Services.VacancyService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BugTrackerTests
{
    public class VacancyControllerTests
    {
        [Fact]
        public void Index_ReturnsARedirect_WhenThereAreNoVacancies()
        {
            // Arrange
            var mockVacancyController = new Mock<IVacancyService>();
            mockVacancyController
                .Setup(vacancyController => vacancyController.GetVacancies())
                .ReturnsAsync(It.IsAny<Func<Vacancy>>());

            var controller = new VacancyController(mockVacancyController.Object);

            const string expectedControllerName = nameof(Issue);
            const string expectedActionName = nameof(IssueController.Index);

            // Act
            var result = controller.Index();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(expectedControllerName, redirectToActionResult.ControllerName);
            Assert.Equal(expectedActionName, redirectToActionResult.ActionName);
        }

        [Fact]
        public void Index_ReturnsAViewResultWithAListOfVacancies_WhenThereAreVacancies()
        {
            // Arrange
            var mockVacancyController = new Mock<IVacancyService>();
            mockVacancyController
                .Setup(vacancyController => vacancyController.GetVacancies())
                .ReturnsAsync(GetTestVacancies());

            var controller = new VacancyController(mockVacancyController.Object);

            var expectedModel = GetTestVacancyViewModels(GetTestVacancies());

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, viewResult.ViewData.Model);
        }

        private static IEnumerable<VacancyViewModel> GetTestVacancyViewModels(Vacancy vacancies)
        {
            var listOfVacancyViewModel = new List<VacancyViewModel>
            {
                new()
                {
                    Title = vacancies.Data.First().Title,
                    CompanyName = vacancies.Data.First().CompanyName,
                    Location = vacancies.Data.First().Location,
                    Relocation = vacancies.Data.First().Remote
                }
            };

            return listOfVacancyViewModel;
        }

        private static Vacancy GetTestVacancies()
        {
            var vacancies = new Vacancy
            {
                Data = new List<Data>
                {
                    new()
                    {
                        Title = "Title",
                        Description = "Description",
                        Location = "City",
                        Remote = true
                    }
                }
            };

            return vacancies;
        }
    }
}