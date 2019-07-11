using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ssyp.Communicator.Api.Storage;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Utilities;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("conversation/send")]
    [ApiController]
    public sealed class ConversationSendController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            Program.Logger.LogDebug("Handling conversation/send request");
            var invalidResult = this.ProcessRequest<ConversationSendRequest>(out var request, out var apiKey);
            Program.Logger.LogDebug($"Parsed the request. Request: {request}");

            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");
            var sender = Program.GetUserByApiKey(apiKey);
            var receiverName = request.Receiver;
            var receiver = Program.GetUserByName(receiverName);

            if (receiver == null)
                return BadRequest($"User {receiverName} can't be found");

            Debug.Assert(Program.DataStorage != null, "Program.DataStorage != null");

            var conversation = Program.DataStorage.Conversations.Find(it =>
            {
                Debug.Assert(sender != null, nameof(sender) + " != null");
                return it.ContainsUser(sender) && it.ContainsUser(receiver);
            });

            if (conversation == null)
            {
                Debug.Assert(sender != null, nameof(sender) + " != null");
                conversation = new Conversation(sender.Name, receiver.Name, new List<Message>());
                Program.DataStorage.Conversations.Add(conversation);
            }

            Debug.Assert(sender != null, nameof(sender) + " != null");
            conversation.Messages.Add(new Message(sender, TimeUtilities.CurrentTimeMillis(), request.Message));
            Program.SaveData();
            return Ok();
        }
    }
}