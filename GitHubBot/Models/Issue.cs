using System.Text.Json.Serialization;

namespace GitHubBot.Models
{
    public class Issue
    {
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }

        [JsonPropertyName("pull_request")]
        public PullRequest PullRequest { get; set; }
    }
}
