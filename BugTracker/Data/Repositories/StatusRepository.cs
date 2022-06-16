using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Repositories
{
    public class StatusRepository : IRepository<Status>
    {
        private readonly ApplicationDbContext _dbContext;

        public StatusRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public IEnumerable<Status> GetAllObjects()
        {
            return _dbContext.Statuses.ToList();
        }

        public Status GetObjectById(int id)
        {
            var result = _dbContext.Statuses.Where(i => i.Id == id);
            return !result.Any() ? null : result.First();
        }

        public void Create(Status item)
        {
            _dbContext.Statuses.Add(item);
        }

        public void Update(Status item)
        {
            var status = GetObjectById(item.Id);

            if (status == null) return;

            status.Name = item.Name;
        }

        public void Delete(int id)
        {
            var status = GetObjectById(id);

            if (status == null) return;
            
            _dbContext.Statuses.Remove(status);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}