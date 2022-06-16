using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Repositories
{
    public class PriorityRepository : IRepository<Priority>
    {
        private readonly ApplicationDbContext _dbContext;

        public PriorityRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public IEnumerable<Priority> GetAllObjects()
        {
            return _dbContext.Priorities.ToList();
        }

        public Priority GetObjectById(int id)
        {
            var result = _dbContext.Priorities.Where(i => i.Id == id);
            return !result.Any() ? null : result.First();
        }

        public void Create(Priority item)
        {
            _dbContext.Priorities.Add(item);
        }

        public void Update(Priority item)
        {
            var priority = GetObjectById(item.Id);

            if (priority == null) return;

            priority.Name = item.Name;
        }

        public void Delete(int id)
        {
            var issue = GetObjectById(id);

            if (issue == null) return;
            
            _dbContext.Priorities.Remove(issue);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}