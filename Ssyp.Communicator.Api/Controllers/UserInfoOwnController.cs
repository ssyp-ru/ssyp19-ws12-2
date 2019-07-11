using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Responses;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/info/own")]
    [ApiController]
    public sealed class UserInfoOwnController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            Program.Logger.LogDebug("Handling user/info/own request");
            var invalidResult = this.ProcessRequest<UserInfoOwnRequest>(out var request, out var apiKey);

            if (invalidResult != null)
                return invalidResult;
            
            Program.Logger.LogDebug($"Parsed the request. Request: {request}");

            var user = Program.GetUserByApiKey(apiKey);
            Debug.Assert(user != null, nameof(user) + " != null");
            var response = new UserInfoOwnResponse(user.Name);
            Program.Logger.LogDebug($"Formed the response. Response: {response}");
            return response.CreateContent();
        }
    }
}