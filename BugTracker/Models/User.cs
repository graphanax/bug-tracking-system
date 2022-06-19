using BugTracker.Data;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models
{
    public class User : IdentityUser, IEntity
    {
        public override string Id { get; set; }

        public string Login { get; set; }
    }
}