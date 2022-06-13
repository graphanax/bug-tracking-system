using System;
using System.Collections.Generic;
using System.Data.Common;

namespace BugTracker.Models.Helpers
{
    public class IssueHelper
    {
        public List<Issue> GetIssues()
        {
            var dateTime = new DateTime();
            var status = new Status {Id = 1, Name = "Assigned"};
            var priority = new Priority {Id = 1, Name = "High"};
            var role = new Role {Id = 1, Name = "TestRole"};
            var user = new User {Id = 1, Email = "test@bug.com", Login = "Account", PasswordHash = "34243", Role = role};
            return new List<Issue>()
            {
                new Issue
                {
                    Id = 1, Title = "Test issue from code", AssignedTo = null, CreatedBy = user, Description = "Test",
                    Created = dateTime, Updated = dateTime, Priority = priority, Status = status
                }
            };
        }
    }
}