using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Helpers
{
    public class PriorityHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public PriorityHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public List<Priority> GetAllPriorities()
        {
            return _dbContext.Priorities.ToList();
        }
    }
}