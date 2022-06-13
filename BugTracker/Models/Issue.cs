#nullable enable
using System;

namespace BugTracker.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public User CreatedBy { get; set; } = null!;
        public User? AssignedTo { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }
}