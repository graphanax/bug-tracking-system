using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Helpers
{
    public class IssueHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public IssueHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Issue> GetAllIssues()
        {
            return _dbContext.Issues.ToList();
        }

        public Issue GetIssueById(int id)
        {
            var result = _dbContext.Issues.Where(i => i.Id == id);
            return !result.Any() ? null : result.First();
        }
    }
}