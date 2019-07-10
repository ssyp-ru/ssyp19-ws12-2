using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
            var invalidResult =
                this.VerifyRequest<ConversationSendRequest>(out var request, out var apiKey);

            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");
            var sender = Program.GetUserByApiKey(apiKey);

            var receiverGuidNullable = request.Receiver.ToGuidOrNull();

            if (!receiverGuidNullable.HasValue)
                return BadRequest();

            var receiver = Program.GetUserByUserID(receiverGuidNullable.Value);

            if (receiver == null)
                return BadRequest();

            Debug.Assert(Program.DataStorage != null, "Program.DataStorage != null");

            var conversation =
                Program.DataStorage.Conversations.Find(it =>
                {
                    Debug.Assert(sender != null, nameof(sender) + " != null");
                    return it.ContainsUser(sender) && it.ContainsUser(receiver);
                });

            if (conversation == null)
            {
                Debug.Assert(sender != null, nameof(sender) + " != null");
                conversation = new Conversation(sender, receiver, new List<Message>());
                Program.DataStorage.Conversations.Add(conversation);
            }

            Debug.Assert(sender != null, nameof(sender) + " != null");
            conversation.Messages.Add(new Message(sender, TimeUtilities.CurrentTimeMillis(), request.Message));
            Program.SaveData();
            return Ok();
        }
    }
}