#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public IEnumerable<User>? Users { get; set; }

        [Required(ErrorMessage = "The priority should be specified")]
        public int PriorityId { get; set; }

        public IEnumerable<Priority>? Priorities { get; set; }

        public override bool Equals(object? o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            return o.GetType() == GetType() && Equals((CreateIssueViewModel) o);
        }

        protected bool Equals(CreateIssueViewModel other)
        {
            return Title == other.Title && Description == other.Description && AssignedToId == other.AssignedToId &&
                   Users.SequenceEqual(other.Users) && PriorityId == other.PriorityId && Priorities.SequenceEqual(other.Priorities);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Description, AssignedToId, Users, PriorityId, Priorities);
        }
    }
}