using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/unregister")]
    [ApiController]
    internal sealed class UserUnregisterController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] [NotNull] string value)
        {
            var invalidResult = this.VerifyRequest<UserModifyRequest>(
                value ?? throw new ArgumentNullException(nameof(value)),
                out var request);

            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");
            var key = request.ApiKey;

            if (!Program.HasUserWithApiKey(key))
                return BadRequest();

            var user = Program.GetUserByApiKey(key);
            Program.DataStorage.Users.Remove(user);
            return Ok();
        }
    }
}