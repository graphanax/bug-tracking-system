using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Helpers
{
    public class StatusHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public StatusHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Status GetStatusByName(string name)
        {
            var result = _dbContext.Statuses.Where(s => s.Name == name);
            return !result.Any() ? null : result.First();
        }
    }
}