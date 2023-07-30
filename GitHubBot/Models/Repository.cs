using System.Text.Json.Serialization;

namespace GitHubBot.Models
{

    public class Repository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; }
    }
}
