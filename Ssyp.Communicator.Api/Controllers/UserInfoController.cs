using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Responses;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/info")]
    [ApiController]
    public sealed class UserInfoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            Program.Logger.LogDebug("Handling user/info request");
            var invalidResult = this.VerifyRequest<UserInfoRequest>(out var request);

            if (invalidResult != null)
                return invalidResult;

            Program.Logger.LogDebug($"Parsed the request. Request: {request}");
            Debug.Assert(request != null, nameof(request) + " != null");
            var userGuidNullable = request.UserID.ToGuidOrNull();

            if (!userGuidNullable.HasValue)
                return BadRequest();

            var userGuid = userGuidNullable.Value;

            if (!Program.HasUserWithUsedID(userGuid))
                return BadRequest();

            var user = Program.GetUserByUserID(userGuid);
            Debug.Assert(user != null, nameof(user) + " != null");
            var response = new UserInfoResponse(user.Name);
            Program.Logger.LogDebug($"Formed the response. Response: {response}");
            return Content(JsonConvert.SerializeObject(response), "application/json");
        }
    }
}