using System.Linq;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using BugTracker.Services;
using BugTracker.Services.VacancyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vacancy = _vacancyService.GetVacancies().Result;

            if (vacancy == null)
                return RedirectToAction(nameof(Index), nameof(Issue));

            var model = vacancy.Data.Select(v => new VacancyListViewModel
            {
                Title = v.Title,
                CompanyName = v.CompanyName,
                Location = v.Location,
                Relocation = v.Remote
            }).ToList();

            return View(model);
        }
    }
}