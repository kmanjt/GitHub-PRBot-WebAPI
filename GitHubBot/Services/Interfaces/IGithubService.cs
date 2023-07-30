namespace GitHubBot.Services.Interfaces
{
    public interface IGitHubService
    {
        Task<string> GetFileContent(string owner, string repo, string path, string commit);
        Task PostComment(string owner, string repo, int pullNumber, string commitId, string path, int position, string comment);
        Task<int?> GetCommentPosition(string owner, string repo, int pullNumber, long commentId);
    }

}
