using System.Linq;
using CuttingEdge.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("conversation/list")]
    [ApiController]
    internal sealed class ConversationListController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] [NotNull] string value)
        {
            Condition.Requires(value, "value").IsNotNull();
            var invalidResult = Requests.VerifyRequest<ConversationListRequest>(value, out var request);

            if (invalidResult != null)
                return invalidResult;

            return Content(JsonConvert.SerializeObject(new ConversationListResponse(Program.DataStorage.Conversations
                .Where(c =>
                    c.First.ApiKey.Equals(request.ApiKey) || c.Second.ApiKey.Equals(request.ApiKey))
                .Select(c => new ConversationListResponse.Conversation(
                    (c.First.ApiKey.Equals(request.ApiKey) ? c.First : c.Second).UserID,
                    c.Messages
                        .Select(m =>
                            new ConversationListResponse.Conversation.Message(m.Sender.UserID, m.Value,
                                m.TimeStamp))
                        .ToList()))
                .ToList())), "application/json");
        }
    }
}