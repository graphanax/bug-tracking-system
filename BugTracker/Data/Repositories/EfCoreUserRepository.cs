using BugTracker.Models;

namespace BugTracker.Data.Repositories
{
    public class EfCoreUserRepository : EfCoreRepository<User, ApplicationDbContext>
    {
        public EfCoreUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}