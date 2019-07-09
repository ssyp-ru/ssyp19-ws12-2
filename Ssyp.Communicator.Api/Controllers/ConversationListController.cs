using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Responses;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("conversation/list")]
    [ApiController]
    public sealed class ConversationListController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            Program.Logger.LogDebug("Handling conversation/list request");
            var invalidResult = this.VerifyRequest<ConversationListRequest>(out var request, out var apiKey);

            if (invalidResult != null)
                return invalidResult;

            Program.Logger.LogDebug($"Parsed the request. Request: {request}");
            Debug.Assert(Program.DataStorage != null, "Program.DataStorage != null");

            var response = new ConversationListResponse(Program.DataStorage.Conversations
                .Where(c =>
                {
                    Debug.Assert(request != null, nameof(request) + " != null");
                    return c.First.ApiKey == apiKey || c.Second.ApiKey == apiKey;
                })
                .Select(c =>
                {
                    Debug.Assert(request != null, nameof(request) + " != null");

                    return new ConversationListResponse.Conversation(
                        (c.First.ApiKey.ToString().Equals(request.ApiKey) ? c.First : c.Second).UserID.ToString(),
                        c.Messages
                            .Select(m =>
                                new ConversationListResponse.Conversation.Message(m.Sender.UserID.ToString(), m.Value,
                                    m.TimeStamp))
                            .ToList());
                })
                .ToList());

            Program.Logger.LogDebug($"Formed the response. Response: {response}");
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }
    }
}