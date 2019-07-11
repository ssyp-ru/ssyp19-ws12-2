using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            Program.Logger.LogDebug("Handling user/unregister request");
            var invalidResult = this.ProcessRequest<UserUnregisterRequest>(out var request, out var apiKey);
            Program.Logger.LogDebug($"Parsed the request. Request: {request}");
            
            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");
            var user = Program.GetUserByApiKey(apiKey);
            Debug.Assert(user != null, nameof(user) + " != null");
            Program.DeleteUser(user);
            return Ok();
        }
    }
}