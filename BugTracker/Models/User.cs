#nullable enable
using System;
using System.ComponentModel.DataAnnotations.Schema;
using BugTracker.Attributes;
using BugTracker.Data;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models
{
    [Table("aspnetusers")]
    [LoginEmailEqual]
    public class User : IdentityUser, IEntity
    {
        public override string Id { get; set; } = null!;

        public string Login { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((User) obj);
        }

        protected bool Equals(User other)
        {
            return Id == other.Id && Login == other.Login;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Login);
        }
    }
}