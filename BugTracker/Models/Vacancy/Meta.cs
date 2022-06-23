namespace BugTracker.Models.Vacancy
{
    public class Meta
    {
        [Newtonsoft.Json.JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        public int From { get; set; }
        public string Path { get; set; }

        [Newtonsoft.Json.JsonProperty("per_page")]
        public int PerPage { get; set; }

        public int To { get; set; }
        public string Terms { get; set; }
        public string Info { get; set; }
    }
}