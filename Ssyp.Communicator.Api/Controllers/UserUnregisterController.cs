using System;
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
        public IActionResult Post([FromBody] [NotNull] string value)
        {
            var invalidResult = this.VerifyRequest<UserModifyRequest>(
                value ?? throw new ArgumentNullException(nameof(value)),
                out var request);

            if (invalidResult != null)
                return invalidResult;

            var key = request.ApiKey;

            if (!Program.HasUserWithApiKey(key))
                return BadRequest();

            var user = Program.GetUserByApiKey(key);
            Program.DataStorage.Users.Remove(user);
            return Ok();
        }
    }
}