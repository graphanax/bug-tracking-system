using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels
{
    public class CreateIssueViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        public int AssignedToId { get; set; }       
        public IEnumerable<User> Users { get; set; }

        [Required]
        public int PriorityId { get; set; }
        public IEnumerable<Priority> Priorities { get; set; }
    }
}