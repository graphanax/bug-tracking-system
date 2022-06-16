#nullable enable
using System;
using System.ComponentModel.DataAnnotations.Schema;
using BugTracker.Data;

namespace BugTracker.Models
{
    public class Issue : IEntity
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        
        [Column("created_by")]
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;

        [Column("assigned_to")]
        public int? AssignedToId { get; set; }
        public virtual User? AssignedTo { get; set; }

        [Column("priority")]
        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; } = null!;

        [Column("status")]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; } = null!;
    }
}