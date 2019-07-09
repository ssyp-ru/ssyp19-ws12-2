using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api
{
    internal static class Requests
    {
        [CanBeNull]
        internal static ActionResult VerifyRequest<TCommunicatorRequest>(
            [NotNull] this ControllerBase controllerBase,
            [NotNull] string value,
            [CanBeNull] out TCommunicatorRequest request) where TCommunicatorRequest : ICommunicatorRequest
        {
            if (!controllerBase.Request.Headers["Content-Type"].Equals("application/json"))
            {
                request = default;
                return controllerBase.BadRequest();
            }

            request = JsonConvert.DeserializeObject<TCommunicatorRequest>(value);

            if (request == null)
                return controllerBase.BadRequest();

            return !Program.HasUserWithApiKey(request.ApiKey) ? controllerBase.StatusCode(403) : null;
        }
    }
}