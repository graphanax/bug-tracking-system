#nullable enable
using System;

namespace BugTracker.Models.ViewModels
{
    public class VacancyViewModel
    {
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public bool Relocation { get; set; }

        public override bool Equals(object? o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            return o.GetType() == GetType() && Equals((VacancyViewModel) o);
        }

        protected bool Equals(VacancyViewModel other)
        {
            return Title == other.Title && CompanyName == other.CompanyName && Location == other.Location &&
                   Relocation == other.Relocation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, CompanyName, Location, Relocation);
        }
    }
}