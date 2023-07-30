using System.Text.Json.Serialization;

namespace GitHubBot.Models
{
    public class File
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; }
    }
}
