using Octokit;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace GitHubBot.Models
{
    public class WebhookPayload
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }

        [JsonPropertyName("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonPropertyName("repository")]
        public Repository Repository { get; set; }
    }
}
