using System.Diagnostics;
using CuttingEdge.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/info")]
    [ApiController]
    internal sealed class UserInfoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] [NotNull] string value)
        {
            Condition.Requires(value, nameof(value)).IsNotNull();
            var invalidResult = this.VerifyRequest<UserInfoRequest>(value, out var request);

            if (invalidResult != null)
                return invalidResult;

            var id = request.UserID;

            if (!Program.HasUserWithUsedID(id))
                return BadRequest();

            var user = Program.GetUserByUserID(request.UserID);
            Debug.Assert(user != null, nameof(user) + " != null");
            return Content(JsonConvert.SerializeObject(new UserInfoResponse(user.Name)), "application/json");
        }
    }
}