using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/modify")]
    [ApiController]
    public sealed class UserModifyController : ControllerBase
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
            Debug.Assert(user != null, nameof(user) + " != null");
            user.Name = request.Name;
            Program.GetUserByApiKey(key);
            return Ok();
        }
    }
}