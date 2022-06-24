using System;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Repositories;
using BugTracker.Models;
using BugTracker.Models.ViewModels.Issue;
using BugTracker.Services.ProducerNotificationService;
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
        private readonly IRabbitMqProducer<NotificationOfIssueAssignment> _rabbitMqProducer;

        public IssueController(ILogger<IssueController> logger,
            EfCoreRepository<Issue, ApplicationDbContext> issueRepository,
            EfCoreRepository<User, ApplicationDbContext> userRepository,
            EfCoreRepository<Status, ApplicationDbContext> statusRepository,
            EfCoreRepository<Priority, ApplicationDbContext> priorityRepository,
            UserManager<User> userManager, IRabbitMqProducer<NotificationOfIssueAssignment> rabbitMqProducer)
        {
            _logger = logger;
            _issueRepository = issueRepository;
            _userRepository = userRepository;
            _statusRepository = statusRepository;
            _priorityRepository = priorityRepository;
            _userManager = userManager;
            _rabbitMqProducer = rabbitMqProducer;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(string searchString = null)
        {
            var model = _issueRepository.GetAllObjects().Result.Select(i => new IssueViewModel
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

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.Title.Contains(searchString)).ToList();
            }

            return View(model);
        }

        // Get request a form to add a new issue
        [HttpGet]
        public IActionResult CreateIssue()
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
        public async Task<IActionResult> CreateIssue(CreateIssueViewModel formData)
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

            if (!string.IsNullOrEmpty(issue.AssignedToId))
                NotifyAssignee(issue.Id, issue.AssignedToId);

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
        public IActionResult EditIssue(int issueId)
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
        public async Task<IActionResult> EditIssue(EditIssueViewModel formData, int issueId)
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

            if (!string.IsNullOrEmpty(issue.AssignedToId))
                NotifyAssignee(issue.Id, issue.AssignedToId);

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

        [NonAction]
        private void NotifyAssignee(int issueId, string assigneeId)
        {
            var assigneeEmail = _userRepository.GetObjectById(assigneeId).Result.Email;
            _logger.LogInformation($"Sending an assignment notification to {assigneeEmail}...");
            _rabbitMqProducer.Publish(new NotificationOfIssueAssignment()
                {IssueId = issueId, AssigneeEmail = assigneeEmail});
        }
    }
}