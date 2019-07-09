using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;

namespace Ssyp.Communicator.Api
{
    internal static class Requests
    {
        [CanBeNull]
        internal static IActionResult VerifyRequest<TCommunicatorRequest>(
            [NotNull] this ControllerBase controllerBase,
            [CanBeNull] out TCommunicatorRequest request,
            out Guid apiKey) where TCommunicatorRequest : ICommunicatorRequest
        {
            if (controllerBase == null)
                throw new ArgumentNullException(nameof(controllerBase));

            request = default;
            apiKey = Guid.Empty;

            var headers = controllerBase.Request.Headers;

            if (!headers["Content-Type"].Equals("application/json") || !headers["Content-Encoding"].Equals("utf-8"))
            {
                request = default;
                apiKey = default;
                return controllerBase.BadRequest();
            }

            try
            {
                request = JsonConvert.DeserializeObject<TCommunicatorRequest>(
                    new StreamReader(controllerBase.Request.Body).ReadToEndAsync().Result);
            }
            catch (JsonException)
            {
                request = default;
                return controllerBase.BadRequest();
            }

            if (request == null)
                return controllerBase.BadRequest();

            var guidNullable = request.ApiKey.ToGuidOrNull();

            if (!guidNullable.HasValue)
                return controllerBase.BadRequest();

            apiKey = guidNullable.Value;

            Program.Logger.LogDebug($"Matching user by key. Inputted: {apiKey}. All users: ");
            Program.DataStorage.Users.ForEach(it => Program.Logger.LogDebug(it.ToString()));
            return !Program.HasUserWithApiKey(apiKey) ? null : controllerBase.StatusCode(403);
        }

        [CanBeNull]
        internal static IActionResult VerifyRequest<TCommunicatorRequest>([NotNull] this ControllerBase controllerBase,
            [CanBeNull] out TCommunicatorRequest request) where TCommunicatorRequest : ICommunicatorRequest =>
            controllerBase.VerifyRequest(out request, out _);
    }
}