using GitHubBot.Models;
using GitHubBot.Services;
using GitHubBot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IOpenAIService _openAiService;
        private readonly IGitHubService _gitHubService;

        public WebhookController(IOpenAIService openAiService, IGitHubService gitHubService)
        {
            _openAiService = openAiService;
            _gitHubService = gitHubService;
        }
        // GET: api/<WebhookController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<WebhookController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<WebhookController>
        [HttpPost]
        public async Task<IActionResult> HandleWebhook([FromBody] WebhookPayload payload)
        {
            // Validate payload
            if (payload.Comment?.Body == null || payload.PullRequest?.Head?.Sha == null || payload.Repository?.Owner?.Login == null || payload.Repository?.Name == null)
            {
                return BadRequest("Invalid payload");
            }

            // Ignore comments made by the bot
            if (payload.Comment.Body.StartsWith("[BOT]")) // modify this line
            {
                return Ok(); // Successfully ignored bot's comment
            }

            try
            {
                var commentBody = payload.Comment.Body;
                var filePath = payload.Comment.Path;
                var commitSha = payload.PullRequest.Head.Sha;
                var codeContext = await _gitHubService.GetFileContent(payload.Repository.Owner.Login, payload.Repository.Name, filePath, commitSha);

                // Get position
                var position = await _gitHubService.GetCommentPosition(payload.Repository.Owner.Login, payload.Repository.Name, payload.PullRequest.Number, payload.Comment.Id);

                if (position == null)
                {
                    return StatusCode(500, "Could not find position in diff");
                }

                var response = await _openAiService.GenerateResponse(commentBody, codeContext, position.Value);


                await _gitHubService.PostComment(
                    payload.Repository.Owner.Login,
                    payload.Repository.Name,
                    payload.PullRequest.Number,
                    commitSha,
                    filePath,
                    position.Value,
                    response);

            }
            catch (Exception e)
            {
                // Log error and return error response
                // _logger.LogError(e, "Error handling webhook");
                return StatusCode(500, "Internal server error");
            }

            return Ok();
        }



        // PUT api/<WebhookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WebhookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
