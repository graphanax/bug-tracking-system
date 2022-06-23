#nullable enable
using System.ComponentModel.DataAnnotations;
using BugTracker.Models;

namespace BugTracker.Attributes
{
    public class LoginEmailEqualAttribute : ValidationAttribute
    {
        public LoginEmailEqualAttribute()
        {
            ErrorMessage = "Login and password must not match";
        }

        public override bool IsValid(object? value)
        {
            return value is User user && user.Login != user.Email;
        }
    }
}