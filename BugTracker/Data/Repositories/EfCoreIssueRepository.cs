using System;
using System.Threading.Tasks;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data.Repositories
{
    public class EfCoreIssueRepository : EfCoreRepository<Issue, ApplicationDbContext>
    {
        public EfCoreIssueRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public override async Task<Issue> Update(Issue item)
        {
            var issue = GetObjectById(item.Id).Result;

            if (issue == null) 
                return null;
            
            issue.Title = string.IsNullOrEmpty(item.Title) ? issue.Title : item.Title;
            issue.Description = string.IsNullOrEmpty(item.Description) ? issue.Description : item.Description;
            issue.PriorityId = item.PriorityId == 0 ? issue.PriorityId : item.PriorityId;
            issue.StatusId = item.StatusId == 0 ? issue.StatusId : item.StatusId;
            issue.AssignedToId = string.IsNullOrEmpty(item.AssignedToId) ? issue.AssignedToId : item.AssignedToId;
            issue.Updated = DateTime.Now;
            
            Context.Entry(issue).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return issue;
        }
    }
}