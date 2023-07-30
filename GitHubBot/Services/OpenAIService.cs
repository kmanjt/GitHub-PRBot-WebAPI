using GitHubBot.Models;
using GitHubBot.Models.Config;
using GitHubBot.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenAI_API.Models;

public class OpenAIService : IOpenAIService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public OpenAIService(IOptions<AppSettings> settings)
    {
        _apiKey = settings.Value.OpenAiApiKey;
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateResponse(string commentBody, string codeContext, int position)
    {
        // Split the code context into lines
        var codeLines = codeContext.Split('\n');

        // Check if the position is valid
        if (position < 0 || position >= codeLines.Length)
        {
            throw new ArgumentException("Invalid position in code context", nameof(position));
        }

        // Extract the line the comment refers to
        var commentLine = codeLines[position];

        // Construct the prompt
        var combinedPrompt =
            $"Write a suggested code improvement for the following comment based on the code in question.\n\n" +
            $"Comment: {commentBody}\n" +
            $"Line of code: {commentLine}\n\n" +
            $"Code: {codeContext}";

        var api = new OpenAI_API.OpenAIAPI(_apiKey);

        var result = await api.Completions.CreateCompletionAsync(combinedPrompt, model: Model.DavinciText, max_tokens: 200);

        return result.ToString();
    }
}
