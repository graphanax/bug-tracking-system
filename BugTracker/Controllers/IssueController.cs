using System;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Repositories;
using BugTracker.Models;
using BugTracker.Models.ViewModels.Issue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BugTracker.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;
        private readonly EfCoreRepository<Issue, ApplicationDbContext> _issueRepository;
        private readonly EfCoreRepository<User, ApplicationDbContext> _userRepository;
        private readonly EfCoreRepository<Status, ApplicationDbContext> _statusRepository;
        private readonly EfCoreRepository<Priority, ApplicationDbContext> _priorityRepository;
        private readonly UserManager<User> _userManager;

        public IssueController(ILogger<IssueController> logger,
            EfCoreRepository<Issue, ApplicationDbContext> issueRepository,
            EfCoreRepository<User, ApplicationDbContext> userRepository,
            EfCoreRepository<Status, ApplicationDbContext> statusRepository,
            EfCoreRepository<Priority, ApplicationDbContext> priorityRepository,
            UserManager<User> userManager)
        {
            _logger = logger;
            _issueRepository = issueRepository;
            _userRepository = userRepository;
            _statusRepository = statusRepository;
            _priorityRepository = priorityRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _issueRepository.GetAllObjects().Result.Select(i => new IssueListViewModel
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

            return View(model);
        }

        // Get request a form to add a new issue
        [HttpGet]
        public ActionResult CreateIssue()
        {
            var model = new CreateIssueViewModel
            {
                Users = _userRepository.GetAllObjects().Result,
                Priorities = _priorityRepository.GetAllObjects().Result
            };

            return View(model);
        }

        // Post request to create a new issue
        [HttpPost]
        public async Task<ActionResult> CreateIssue(CreateIssueViewModel formData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CreateIssue));

            var issue = new Issue
            {
                Title = formData.Title,
                Description = formData.Description,
                Created = DateTime.Now,
                CreatedById = _userManager.GetUserAsync(User).Result.Id,
                AssignedToId = formData.AssignedToId,
                PriorityId = formData.PriorityId,
                Status = _statusRepository.GetObjectById(1).Result
            };

            await _issueRepository.Create(issue);
            _logger.LogInformation($"Issue #{issue.Id} has been created.");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DetailIssue(int issueId)
        {
            var issue = _issueRepository.GetObjectById(issueId).Result;

            if (issue == null)
                return RedirectToAction(nameof(Index));

            var model = new IssueDetailsViewModel
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Created = issue.Created,
                Updated = issue.Updated,
                CreatedBy = issue.CreatedBy.Login,
                AssignedTo = issue.AssignedTo?.Login,
                Priority = issue.Priority.Name,
                Status = issue.Status.Name
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIssueStatus(int issueId, int statusId)
        {
            var issue = new Issue
            {
                Id = issueId,
                StatusId = statusId,
                Updated = DateTime.Now
            };

            await _issueRepository.Update(issue);
            _logger.LogInformation($"Status of issue #{issue.Id} has been updated to #{issue.StatusId}.");

            return RedirectToAction(nameof(DetailIssue), issueId);
        }

        // Get request a form to edit issue
        [HttpGet]
        public ActionResult EditIssue(int issueId)
        {
            var issue = _issueRepository.GetObjectById(issueId).Result;

            if (issue == null)
                return RedirectToAction(nameof(Index));

            var model = new EditIssueViewModel
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                PriorityId = issue.PriorityId,
                Priorities = _priorityRepository.GetAllObjects().Result,
                AssignedToId = issue.AssignedToId,
                Users = _userRepository.GetAllObjects().Result
            };

            return View(model);
        }

        // Post request to edit issue
        [HttpPost]
        public async Task<ActionResult> EditIssue(EditIssueViewModel formData, int issueId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditIssue), issueId);

            var issue = new Issue
            {
                Id = formData.Id,
                Title = formData.Title,
                Description = formData.Description,
                PriorityId = formData.PriorityId,
                AssignedToId = formData.AssignedToId,
                Updated = DateTime.Now
            };

            await _issueRepository.Update(issue);
            _logger.LogInformation($"Issue #{issue.Id} has been updated.");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIssue(int issueId)
        {
            var issue = _issueRepository.GetObjectById(issueId).Result;

            if (issue != null)
            {
                await _issueRepository.Delete(issue.Id);
                _logger.LogInformation($"Issue #{issue.Id} has been created.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}