#nullable enable
using System;

namespace BugTracker.Models.ViewModels
{
    public class IssueListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string CreatedBy { get; set; }
        public string? AssignedTo { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}