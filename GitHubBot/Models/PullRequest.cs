using System.Text.Json.Serialization;

namespace GitHubBot.Models
{
    public class PullRequest
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("head")]
        public Head Head { get; set; }
    }
}
