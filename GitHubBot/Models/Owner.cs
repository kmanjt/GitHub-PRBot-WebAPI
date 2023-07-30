using System.Text.Json.Serialization;

namespace GitHubBot.Models
{
    public class Owner
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
    }
}
