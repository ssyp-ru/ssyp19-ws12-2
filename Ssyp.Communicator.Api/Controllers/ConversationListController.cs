using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var invalidResult = this.ProcessRequest<ConversationListRequest>(out var request, out var apiKey);

            if (invalidResult != null)
                return invalidResult;

            Program.Logger.LogDebug($"Parsed the request. Request: {request}");
            Debug.Assert(Program.DataStorage != null, "Program.DataStorage != null");

            var response = new ConversationListResponse(Program.DataStorage.Conversations
                .Where(c =>
                {
                    Debug.Assert(request != null, nameof(request) + " != null");

                    return Program.GetUserByName(c.First)?.ApiKey == apiKey ||
                           Program.GetUserByName(c.Second)?.ApiKey == apiKey;
                })
                .Select(c =>
                {
                    Debug.Assert(request != null, nameof(request) + " != null");
                    var interlocutor = Program.GetUserByName(c.First);
                    Debug.Assert(interlocutor != null, nameof(interlocutor) + " != null");

                    return new ConversationListResponse.Conversation(
                        (interlocutor.ApiKey.ToString().Equals(request.ApiKey)
                            ? Program.GetUserByName(c.First)
                            : Program.GetUserByName(c.Second))?.Name.ToString(),
                        c.Messages
                            .Select(m =>
                            {
                                var sender = Program.GetUserByName(m.Sender);

                                Debug.Assert(sender != null, nameof(sender) + " != null");
                                return new ConversationListResponse.Conversation.Message(sender.Name.ToString(),
                                    m.Value,
                                    m.TimeStamp);
                            })
                            .ToList());
                })
                .ToList());

            Program.Logger.LogDebug($"Formed the response. Response: {response}");
            return response.CreateContent();
        }
    }
}