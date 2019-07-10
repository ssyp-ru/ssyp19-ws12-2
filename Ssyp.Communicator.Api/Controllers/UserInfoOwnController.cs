using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Responses;

namespace Ssyp.Communicator.Api.Controllers
{
    public sealed class UserInfoOwnController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            Program.Logger.LogDebug("Handling user/info request");
            var invalidResult = this.VerifyRequest<UserInfoRequest>(out Guid apiKey);

            if (invalidResult != null)
                return invalidResult;

            var user = Program.GetUserByApiKey(apiKey);

            Debug.Assert(user != null, nameof(user) + " != null");
            var response = new UserInfoOwnResponse(user.UserID.ToString(), user.Name);

            return Content(JsonConvert.SerializeObject(),)
        }
    }
}