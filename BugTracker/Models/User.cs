using System.ComponentModel.DataAnnotations.Schema;
using BugTracker.Data;

namespace BugTracker.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        [Column("role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}