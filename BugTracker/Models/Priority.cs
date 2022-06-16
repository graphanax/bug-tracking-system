using BugTracker.Data;

namespace BugTracker.Models
{
    public class Priority : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}