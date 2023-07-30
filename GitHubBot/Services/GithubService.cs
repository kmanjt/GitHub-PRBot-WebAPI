using GitHubBot.Models.Config;
using GitHubBot.Services.Interfaces;
using Microsoft.Extensions.Options;
using Octokit;
using System.IO;

namespace GitHubBot.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _github;

        public GitHubService(IOptions<AppSettings> settings)
        {
            _github = new GitHubClient(new ProductHeaderValue("MyApp"))
            {
                Credentials = new Credentials(settings.Value.GitHubToken)
            };
        }


        public async Task<string> GetFileContent(string owner, string repo, string path, string commit)
        {
            var contentsResponse = await _github.Repository.Content.GetAllContentsByRef(owner, repo, path, commit);
            var fileContent = contentsResponse.First().Content;

            return fileContent;
        }

        public async Task PostComment(string owner, string repo, int pullNumber, string commitId, string path, int position, string comment)
        {
            string prefix = "Suggested code improvement:";
            string codeSuggestion = comment.Replace(prefix, "").Trim(); // remove prefix from the original comment
            string suggestion = $"[BOT] {prefix}\n```suggestion\n{codeSuggestion}\n```"; // re-add prefix in the formatted comment
            var newComment = new PullRequestReviewCommentCreate(suggestion, commitId, path, position);
            await _github.PullRequest.ReviewComment.Create(owner, repo, pullNumber, newComment);
        }

        public async Task<int?> GetCommentPosition(string owner, string repo, int pullNumber, long commentId)
        {
            try
            {
                // Get the pull request comments
                var comments = await _github.PullRequest.ReviewComment.GetAll(owner, repo, pullNumber);

                // Find the specific comment
                var comment = comments.FirstOrDefault(c => c.Id == commentId);

                if (comment == null)
                {
                    return null;
                }

                // Return the original position
                return comment.OriginalPosition - 2;

            }
            catch (Exception)
            {
                // Log the exception and return null or throw an appropriate exception
                return null;
            }
        }



    }
}
