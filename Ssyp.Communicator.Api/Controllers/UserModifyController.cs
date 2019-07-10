using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ssyp.Communicator.Common.Requests;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/modify")]
    [ApiController]
    public sealed class UserModifyController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var invalidResult = this.VerifyRequest<UserModifyRequest>(out var request, out var apiKey);

            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");

            var user = Program.GetUserByApiKey(apiKey);
            Debug.Assert(user != null, nameof(user) + " != null");
            user.Name = request.Name;
            Program.GetUserByApiKey(apiKey);
            return Ok();
        }
    }
}