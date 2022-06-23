using System.Collections.Generic;

namespace BugTracker.Models.Vacancy
{
    public class Vacancy
    {
        public List<Data> Data { get; set; }
        public Links Links { get; set; }
        public Meta Meta { get; set; }
    }
}