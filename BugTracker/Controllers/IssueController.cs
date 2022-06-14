using System.Linq;
using BugTracker.Data;
using BugTracker.Models.Helpers;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueHelper _issueHelper;

        public IssueController(ApplicationDbContext applicationDbContext)
        {
            _issueHelper = new IssueHelper(applicationDbContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _issueHelper.GetAllIssues().Select(x => new IssueListViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Created = x.Created,
                Updated = x.Updated,
                CreatedBy = x.CreatedBy.Login,
                AssignedTo = x.AssignedTo?.Login,
                Priority = x.Priority.Name,
                Status = x.Status.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult DetailIssue(int issueId)
        {
            var issue = _issueHelper.GetIssueById(issueId);

            if (issue == null)
                return RedirectToAction("Index");
            
            var model = new IssueListViewModel
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
    }
}