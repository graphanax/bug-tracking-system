using System;
using System.Linq;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.Helpers;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class IssueController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IssueHelper _issueHelper;
        private readonly UserHelper _userHelper;
        private readonly StatusHelper _statusHelper;
        private readonly PriorityHelper _priorityHelper;

        public IssueController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _issueHelper = new IssueHelper(dbContext);
            _userHelper = new UserHelper(dbContext);
            _statusHelper = new StatusHelper(dbContext);
            _priorityHelper = new PriorityHelper(dbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _issueHelper.GetAllIssues().Select(i => new IssueListViewModel
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
                Users = _userHelper.GetAllUsers(),
                Priorities = _priorityHelper.GetAllPriorities()
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
                Status = _statusHelper.GetStatusByName("Open")
            };

            _dbContext.Issues.Add(issue);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DetailIssue(int issueId)
        {
            var issue = _issueHelper.GetIssueById(issueId);

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
        public IActionResult ChangeIssueStatus(int issueId, string statusName)
        {
            var issue = _issueHelper.GetIssueById(issueId);
            var status = _statusHelper.GetStatusByName(statusName);

            if (issue == null || status == null)
                return RedirectToAction(nameof(Index));

            issue.Status = status;
            issue.Updated = DateTime.Now;
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(DetailIssue), issueId);
        }

        // Get request a form to edit issue
        [HttpGet]
        public ActionResult EditIssue(int issueId)
        {
            var issue = _issueHelper.GetIssueById(issueId);

            if (issue == null)
                return RedirectToAction(nameof(Index));

            var model = new EditIssueViewModel
            {
                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                PriorityId = issue.PriorityId,
                Priorities = _priorityHelper.GetAllPriorities(),
                AssignedToId = issue.AssignedToId,
                Users = _userHelper.GetAllUsers()
            };

            return View(model);
        }

        // Post request to edit issue
        [HttpPost]
        public ActionResult EditIssue(EditIssueViewModel formData, int issueId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(EditIssue), issueId);

            var issue = _issueHelper.GetIssueById(issueId);
            
            if (issue == null)
                return RedirectToAction(nameof(Index));

            issue.Title = formData.Title;
            issue.Description = formData.Description;
            issue.PriorityId = formData.PriorityId;
            issue.AssignedToId = formData.AssignedToId;
            issue.Updated = DateTime.Now;

            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult DeleteIssue(int issueId)
        {
            var issue = _issueHelper.GetIssueById(issueId);

            if (issue == null)
                return RedirectToAction(nameof(Index));

            _dbContext.Issues.Remove(issue);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}