﻿#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels.Issue
{
    public class EditIssueViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} of issue is not specified")]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "The length of the {0} should be from {2} to {1} characters")]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? AssignedToId { get; set; }
        public IEnumerable<User>? Users { get; set; }

        [Required(ErrorMessage = "The {0} should be specified")]
        public int PriorityId { get; set; }

        public IEnumerable<Priority>? Priorities { get; set; } = null!;
    }
}