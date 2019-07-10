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
            Program.Logger.LogDebug("Handling user/info request");
            var invalidResult = this.VerifyRequest<UserInfoOwnRequest>(out Guid apiKey);

            if (invalidResult != null)
                return invalidResult;

            var user = Program.GetUserByApiKey(apiKey);
            Debug.Assert(user != null, nameof(user) + " != null");
            var response = new UserInfoOwnResponse(user.Name.ToString(), user.Name);
            return response.CreateContent();
        }
    }
}