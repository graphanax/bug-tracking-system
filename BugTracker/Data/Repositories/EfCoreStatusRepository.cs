using BugTracker.Models;

namespace BugTracker.Data.Repositories
{
    public class EfCoreStatusRepository : EfCoreRepository<Status, ApplicationDbContext>
    {
        public EfCoreStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}