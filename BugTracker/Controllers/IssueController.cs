using System;
using System.Linq;
using BugTracker.Models;
using BugTracker.Models.Helpers;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class IssueController : Controller
    {
        private IssueHelper _issueHelper;

        public IssueController()
        {
            _issueHelper = new IssueHelper();
        }

        // GET
        public IActionResult Index()
        {
            var model = _issueHelper.GetIssues().Select(x => new IssueListViewModel
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
        
        // TODO: private string GetFormattedDate
    }
}