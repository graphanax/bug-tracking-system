using System;
using System.Linq;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class IssueController : Controller
    {
        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Status> _statusRepository;
        private readonly IRepository<Priority> _priorityRepository;

        public IssueController(IRepository<Issue> issueRepository, IRepository<User> userRepository,
            IRepository<Status> statusRepository, IRepository<Priority> priorityRepository)
        {
            _issueRepository = issueRepository;
            _userRepository = userRepository;
            _statusRepository = statusRepository;
            _priorityRepository = priorityRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _issueRepository.GetAllObjects().Select(i => new IssueListViewModel
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
                Users = _userRepository.GetAllObjects(),
                Priorities = _priorityRepository.GetAllObjects()
            };

            return View(model);
        }

        // Post request to create a new issue
        [HttpPost]
        public ActionResult CreateIssue(CreateIssueViewModel formData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CreateIssue));

            var issue = new Issue
            {
                Title = formData.Title,
                Description = formData.Description,
                Created = DateTime.Now,
                CreatedById = 1,
                AssignedToId = formData.AssignedToId,
                PriorityId = formData.PriorityId,
                Status = _statusRepository.GetObjectById(1)
            };

            _issueRepository.Create(issue);
            _issueRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DetailIssue(int issueId)
        {
            var issue = _issueRepository.GetObjectById(issueId);

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
        public IActionResult ChangeIssueStatus(int issueId, int statusId)
        {
            var issue = new Issue
            {
                Id = issueId,
                StatusId = statusId,
                Updated = DateTime.Now
            };
            _issueRepository.Update(issue);
            _issueRepository.Save();

            return RedirectToAction(nameof(DetailIssue), issueId);
        }

        // Get request a form to edit issue
        [HttpGet]
        public ActionResult EditIssue(int issueId)
        {
            var issue = _issueRepository.GetObjectById(issueId);

            if (issue == null)
                return RedirectToAction(nameof(Index));

            var model = new EditIssueViewModel
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                PriorityId = issue.PriorityId,
                Priorities = _priorityRepository.GetAllObjects(),
                AssignedToId = issue.AssignedToId,
                Users = _userRepository.GetAllObjects()
            };

            return View(model);
        }

        // Post request to edit issue
        [HttpPost]
        public ActionResult EditIssue(EditIssueViewModel formData, int issueId)
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

            _issueRepository.Update(issue);
            _issueRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeleteIssue(int issueId)
        {
            _issueRepository.Delete(issueId);
            _issueRepository.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}