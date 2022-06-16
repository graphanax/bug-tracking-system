using BugTracker.Data;

namespace BugTracker.Models
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}