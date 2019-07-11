using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            Program.Logger.LogDebug("Handling user/modify request");
            var invalidResult = this.ProcessRequest<UserModifyRequest>(out var request, out var apiKey);
            Program.Logger.LogDebug($"Parsed the request. Request: {request}");
            
            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");
            var name = request.Name;
            var user = Program.GetUserByApiKey(apiKey);

            Debug.Assert(user != null, nameof(user) + " != null");

            if (!Program.RenameUser(user, request.Name)) 
                return BadRequest($"Name {name} can't be used as user name");
            
            Program.SaveData();
            return Ok();

        }
    }
}