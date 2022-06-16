using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;

namespace BugTracker.Models.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public IEnumerable<User> GetAllObjects()
        {
            return _dbContext.Users.ToList();
        }

        public User GetObjectById(int id)
        {
            var result = _dbContext.Users.Where(i => i.Id == id);
            return !result.Any() ? null : result.First();
        }

        public void Create(User item)
        {
            _dbContext.Users.Add(item);
        }

        public void Update(User item)
        {
            var user = GetObjectById(item.Id);

            if (user == null) return;
            
            user.Email = item.Email;
            user.Password = item.Password;
            user.RoleId = item.RoleId;
        }

        public void Delete(int id)
        {
            var user = GetObjectById(id);

            if (user == null) return;
            
            _dbContext.Users.Remove(user);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}