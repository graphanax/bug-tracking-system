using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Helpers
{
    public class UserHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public UserHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetIssueByLogin(string login)
        {
            var result = _dbContext.Users.Where(i => i.Login == login);
            return !result.Any() ? null : result.First();
        }
    }
}