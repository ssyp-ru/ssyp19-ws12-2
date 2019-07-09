using System;
using System.Diagnostics;
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
    public sealed class ConversationListController : ControllerBase
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

            Debug.Assert(Program.DataStorage != null, "Program.DataStorage != null");

            var response = new ConversationListResponse(Program.DataStorage.Conversations
                .Where(c =>
                {
                    Debug.Assert(request != null, nameof(request) + " != null");
                    return c.First.ApiKey.Equals(request.ApiKey) || c.Second.ApiKey.Equals(request.ApiKey);
                })
                .Select(c =>
                {
                    Debug.Assert(request != null, nameof(request) + " != null");

                    return new ConversationListResponse.Conversation(
                        (c.First.ApiKey.Equals(request.ApiKey) ? c.First : c.Second).UserID,
                        c.Messages
                            .Select(m =>
                                new ConversationListResponse.Conversation.Message(m.Sender.UserID, m.Value,
                                    m.TimeStamp))
                            .ToList());
                })
                .ToList());

            Program.Logger.LogDebug($"Formed response. Response: {response}");

            return Content(JsonConvert.SerializeObject(response), "application/json");
        }
    }
}