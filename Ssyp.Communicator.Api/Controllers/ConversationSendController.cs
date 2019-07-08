using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Ssyp.Communicator.Api.Storage;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("conversation/send")]
    [ApiController]
    internal sealed class ConversationSendController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] [NotNull] string value)
        {
            var invalidResult =
                this.VerifyRequest<ConversationSendRequest>(value ?? throw new ArgumentNullException(nameof(value)),
                    out var request);

            if (invalidResult != null)
                return invalidResult;

            var sender = Program.GetUserByApiKey(request.ApiKey);
            var receiver = Program.GetUserByUserID(request.Receiver);

            if (sender == null || receiver == null)
                return BadRequest();

            var conversation =
                Program.DataStorage.Conversations.Find(it => it.ContainsUser(sender) && it.ContainsUser(receiver));

            if (conversation == null)
            {
                conversation = new Conversation(sender, receiver, new List<Message>());
                Program.DataStorage.Conversations.Add(conversation);
            }

            conversation.Messages.Add(new Message(sender, Stopwatch.GetTimestamp(), request.Message));
            return Ok();
        }
    }
}