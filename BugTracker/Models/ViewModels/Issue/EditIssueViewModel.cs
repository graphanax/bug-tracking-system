using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels.Issue
{
    public class EditIssueViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title of issue is not specified")]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "The length of the title should be from 5 to 100 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        public int? AssignedToId { get; set; }
        public IEnumerable<User> Users { get; set; }

        [Required(ErrorMessage = "The priority should be specified")]
        public int PriorityId { get; set; }

        public IEnumerable<Priority> Priorities { get; set; }
    }
}