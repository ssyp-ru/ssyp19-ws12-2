using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api.Controllers
{
    [Route("user/info")]
    [ApiController]
    public sealed class UserInfoController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] [NotNull] string value)
        {
            var invalidResult =
                this.VerifyRequest<UserInfoRequest>(
                    value ?? throw new ArgumentNullException(nameof(value)),
                    out var request);

            if (invalidResult != null)
                return invalidResult;

            Debug.Assert(request != null, nameof(request) + " != null");
            var id = request.UserID;

            if (!Program.HasUserWithUsedID(id))
                return BadRequest();

            var user = Program.GetUserByUserID(request.UserID);
            Debug.Assert(user != null, nameof(user) + " != null");
            return Content(JsonConvert.SerializeObject(new UserInfoResponse(user.Name)), "application/json");
        }
    }
}