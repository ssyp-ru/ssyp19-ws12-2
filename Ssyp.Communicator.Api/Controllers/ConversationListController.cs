using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("conversation/list")]
    [ApiController]
    internal sealed class ConversationListController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] [NotNull] string value)
        {
            Program.Logger.LogDebug("Handling conversation/list request");

            var invalidResult = this.VerifyRequest<ConversationListRequest>(
                value ?? throw new ArgumentNullException(nameof(value)),
                out var request);

            if (invalidResult != null)
                return invalidResult;

            Program.Logger.LogDebug($"Parsed the request. Request: {request}");

            var response = new ConversationListResponse(Program.DataStorage.Conversations
                .Where(c =>
                    c.First.ApiKey.Equals(request.ApiKey) || c.Second.ApiKey.Equals(request.ApiKey))
                .Select(c => new ConversationListResponse.Conversation(
                    (c.First.ApiKey.Equals(request.ApiKey) ? c.First : c.Second).UserID,
                    c.Messages
                        .Select(m =>
                            new ConversationListResponse.Conversation.Message(m.Sender.UserID, m.Value,
                                m.TimeStamp))
                        .ToList()))
                .ToList());

            Program.Logger.LogDebug($"Formed response. Response: {response}");

            return Content(JsonConvert.SerializeObject(response), "application/json");
        }
    }
}