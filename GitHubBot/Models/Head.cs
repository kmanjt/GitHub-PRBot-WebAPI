using System.Text.Json.Serialization;

namespace GitHubBot.Models
{
    public class Head
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; }
    }
}
