using System.Diagnostics;
using CuttingEdge.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/modify")]
    [ApiController]
    internal sealed class UserModifyController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] [NotNull] string value)
        {
            Condition.Requires(value, "value").IsNotNull();
            var invalidResult = Requests.VerifyRequest<UserModifyRequest>(value, out var request);

            if (invalidResult != null)
                return invalidResult;

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