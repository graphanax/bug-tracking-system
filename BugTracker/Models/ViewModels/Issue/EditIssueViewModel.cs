#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BugTracker.Models.ViewModels.Issue
{
    public class EditIssueViewModel
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "The {0} of issue is not specified")]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "The length of the {0} should be from {2} to {1} characters")]
        public string Title { get; init; } = null!;

        public string Description { get; init; } = null!;

        public string? AssignedToId { get; init; }
        public IEnumerable<User>? Users { get; init; }

        [Required(ErrorMessage = "The {0} should be specified")]
        public int PriorityId { get; init; }

        public IEnumerable<Priority>? Priorities { get; init; }

        public override bool Equals(object? o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            return o.GetType() == GetType() && Equals((EditIssueViewModel) o);
        }

        protected bool Equals(EditIssueViewModel other)
        {
            return other.Priorities != null && Priorities != null && other.Users != null && Users != null &&
                   Id == other.Id && Title == other.Title && Description == other.Description &&
                   AssignedToId == other.AssignedToId && Users.SequenceEqual(other.Users) &&
                   PriorityId == other.PriorityId && Priorities.SequenceEqual(other.Priorities);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Description, AssignedToId, Users, PriorityId, Priorities);
        }
    }
}