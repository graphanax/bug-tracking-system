using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Repositories
{
    public class IssueRepository : IRepository<Issue>
    {
        private readonly ApplicationDbContext _dbContext;

        public IssueRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public IEnumerable<Issue> GetAllObjects()
        {
            return _dbContext.Issues.ToList();
        }

        public Issue GetObjectById(int id)
        {
            var result = _dbContext.Issues.Where(i => i.Id == id);
            return !result.Any() ? null : result.First();
        }

        public void Create(Issue item)
        {
            _dbContext.Issues.Add(item);
        }

        public void Update(Issue item)
        {
            var issue = GetObjectById(item.Id);

            if (issue == null) return;
            
            issue.Title = string.IsNullOrEmpty(item.Title) ? issue.Title : item.Title;
            issue.Description = string.IsNullOrEmpty(item.Description) ? issue.Description : item.Description;
            issue.PriorityId = item.PriorityId == 0 ? issue.PriorityId : item.PriorityId;
            issue.StatusId = item.StatusId == 0 ? issue.StatusId : item.StatusId;
            issue.AssignedToId = item.AssignedToId == 0 ? issue.AssignedToId : item.AssignedToId;
            issue.Updated = DateTime.Now;
        }

        public void Delete(int id)
        {
            var issue = GetObjectById(id);

            if (issue == null) return;
            
            _dbContext.Issues.Remove(issue);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}