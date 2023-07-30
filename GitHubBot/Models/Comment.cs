using System.Text.Json.Serialization;

namespace GitHubBot.Models
{
    public class Comment
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
