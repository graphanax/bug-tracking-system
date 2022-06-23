using System.ComponentModel.DataAnnotations;
using BugTracker.Models;

namespace BugTracker.Attributes
{
    public class UpdateDateIsEarlierThanAdditionDateAttribute : ValidationAttribute
    {
        public UpdateDateIsEarlierThanAdditionDateAttribute()
        {
            ErrorMessage = "The update date cannot be earlier than the date of addition";
        }
        
        public override bool IsValid(object value)
        {
            return value is Issue issue && issue.Created <= issue.Updated;
        }
    }
}