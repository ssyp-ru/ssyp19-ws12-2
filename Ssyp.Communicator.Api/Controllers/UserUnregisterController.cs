using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ssyp.Communicator.Common.Requests;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/unregister")]
    [ApiController]
    public sealed class UserUnregisterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var invalidResult = this.VerifyRequest<UserUnregisterRequest>(out var request, out var apiKey);

            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");

            if (!Program.HasUserWithApiKey(apiKey))
                return BadRequest();

            var user = Program.GetUserByApiKey(apiKey);
            Debug.Assert(Program.DataStorage != null, "Program.DataStorage != null");
            Program.DataStorage.Users.Remove(user);
            return Ok();
        }
    }
}