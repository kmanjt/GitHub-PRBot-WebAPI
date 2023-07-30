namespace GitHubBot.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GenerateResponse(string commentBody, string codeContext, int position);
    }
}