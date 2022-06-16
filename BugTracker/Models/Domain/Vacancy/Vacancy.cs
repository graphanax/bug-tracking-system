using System.Collections.Generic;

namespace BugTracker.Models.Domain.Vacancy
{
    public class Vacancy
    {
        public List<Data> Data { get; set; }
        public Links Links { get; set; }
        public Meta Meta { get; set; }
    }
}