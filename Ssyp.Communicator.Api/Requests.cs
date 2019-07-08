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
            [NotNull] string value,
            [NotNull] out TCommunicatorRequest request) where TCommunicatorRequest : ICommunicatorRequest
        {
            Condition.Requires(value, "value").IsNotNull();
            request = JsonConvert.DeserializeObject<TCommunicatorRequest>(value);

            if (request == null)
                return new BadRequestResult();

            return !Program.HasUserWithApiKey(request.ApiKey) ? new StatusCodeResult(403) : null;
        }
    }
}