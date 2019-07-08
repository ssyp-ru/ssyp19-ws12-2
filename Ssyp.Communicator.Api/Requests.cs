using CuttingEdge.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ssyp.Communicator.Common;

namespace Ssyp.Communicator.Api
{
    internal static class Requests
    {
        [CanBeNull]
        internal static IActionResult VerifyRequest<TCommunicatorRequest>(
            [NotNull] this ControllerBase controllerBase,
            [NotNull] string value,
            [NotNull] out TCommunicatorRequest request) where TCommunicatorRequest : ICommunicatorRequest
        {
            Condition.Requires(controllerBase, nameof(controllerBase)).IsNotNull();
            Condition.Requires(value, nameof(value)).IsNotNull();
            request = JsonConvert.DeserializeObject<TCommunicatorRequest>(value);

            if (request == null)
                return controllerBase.BadRequest();

            return !Program.HasUserWithApiKey(request.ApiKey) ? controllerBase.StatusCode(403) : null;
        }
    }
}