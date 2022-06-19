#nullable enable
using System;
using BugTracker.Data;

namespace BugTracker.Models
{
    public class Issue : IEntity
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        
        public string CreatedById { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        
        public string? AssignedToId { get; set; }
        public virtual User? AssignedTo { get; set; }
        
        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; } = null!;
        
        public int StatusId { get; set; }
        public virtual Status Status { get; set; } = null!;
    }
}