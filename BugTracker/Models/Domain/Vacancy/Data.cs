using System.Collections.Generic;

namespace BugTracker.Models.Domain.Vacancy
{
    public class Data
    {
        public string Slug { get; set; }

        [Newtonsoft.Json.JsonProperty("company_name")]
        public string CompanyName { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Remote { get; set; }
        public string Url { get; set; }
        public List<string> Tags { get; set; }

        [Newtonsoft.Json.JsonProperty("job_types")]
        public List<string> JobTypes { get; set; }

        public string Location { get; set; }

        [Newtonsoft.Json.JsonProperty("created_at")]
        public int CreatedAt { get; set; }
    }
}