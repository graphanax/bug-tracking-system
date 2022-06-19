#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels.Issue
{
    public class CreateIssueViewModel
    {
        [Required(ErrorMessage = "The title of issue is not specified")]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "The length of the title should be from 5 to 100 characters")]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? AssignedToId { get; set; }
        public IEnumerable<User>? Users { get; set; } = null!;

        [Required(ErrorMessage = "The priority should be specified")]
        public int PriorityId { get; set; }

        public IEnumerable<Priority>? Priorities { get; set; } = null!;
    }
}