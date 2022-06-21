#nullable enable
using System;
using BugTracker.Data;

namespace BugTracker.Models
{
    public class Status : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Status) obj);
        }

        protected bool Equals(Status other)
        {
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}