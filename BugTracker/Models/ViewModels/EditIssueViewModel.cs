using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels
{
    public class EditIssueViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        public int? AssignedToId { get; set; }
        public List<User> Users { get; set; }

        [Required]
        public int PriorityId { get; set; }
        public List<Priority> Priorities { get; set; }
    }
}