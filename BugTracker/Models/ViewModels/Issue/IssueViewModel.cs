﻿#nullable enable
using System;

namespace BugTracker.Models.ViewModels.Issue
{
    public class IssueViewModel
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public DateTime Created { get; init; }
        public DateTime? Updated { get; init; }
        public string CreatedBy { get; init; } = null!;
        public string? AssignedTo { get; init; }
        public string Priority { get; init; } = null!;
        public string Status { get; init; } = null!;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((IssueViewModel) obj);
        }

        protected bool Equals(IssueViewModel other)
        {
            return Id == other.Id && Title == other.Title && Description == other.Description &&
                   Created.Equals(other.Created) && Nullable.Equals(Updated, other.Updated) &&
                   CreatedBy == other.CreatedBy && AssignedTo == other.AssignedTo && Priority == other.Priority &&
                   Status == other.Status;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(Title);
            hashCode.Add(Description);
            hashCode.Add(Created);
            hashCode.Add(Updated);
            hashCode.Add(CreatedBy);
            hashCode.Add(AssignedTo);
            hashCode.Add(Priority);
            hashCode.Add(Status);
            return hashCode.ToHashCode();
        }
    }
}