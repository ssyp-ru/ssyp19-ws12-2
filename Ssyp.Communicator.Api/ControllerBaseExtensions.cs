using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;

namespace Ssyp.Communicator.Api
{
    internal static class ControllerBaseExtensions
    {
        [CanBeNull]
        internal static IActionResult ProcessRequest<TCommunicatorRequest>(
            [NotNull] this ControllerBase controllerBase,
            [CanBeNull] out TCommunicatorRequest request,
            out Guid apiKey) where TCommunicatorRequest : class, ICommunicatorRequest
        {
            if (controllerBase == null)
                throw new ArgumentNullException(nameof(controllerBase));

            Program.Logger.LogDebug("Processing request");

            request = null;
            apiKey = Guid.Empty;

            if (!HeadersValid(controllerBase.Request.Headers))
                return controllerBase.BadRequest(
                    $"The request has invalid encoding (not {Encoding.UTF8.WebName}) or invalid content type " +
                    $"(not {MediaTypeNames.Application.Json})");

            Program.Logger.LogDebug("Headers are valid");

            if (!ParseBody(controllerBase, out request, out var actionResult))
                return actionResult;

            Program.Logger.LogDebug("JSON is valid");

            var guidString = request.ApiKey;
            var guidNullable = request.ApiKey.ToGuidOrNull();

            if (!guidNullable.HasValue)
                return controllerBase.BadRequest($"String {guidString} can't be parsed as GUID");

            Program.Logger.LogDebug("GUID is valid");
            apiKey = guidNullable.Value;
            Program.Logger.LogDebug($"Matching user by key. Inputted: {apiKey}. All users: ");
            Program.DataStorage.Users.ForEach(it => Program.Logger.LogDebug(it.ToString()));
            return Program.HasUserWithApiKey(apiKey) ? null : controllerBase.StatusCode(403);
        }

        private static bool ParseBody<TCommunicatorRequest>(ControllerBase controllerBase,
            out TCommunicatorRequest request,
            out IActionResult actionResult) where TCommunicatorRequest : class, ICommunicatorRequest
        {
            try
            {
                request = JsonConvert.DeserializeObject<TCommunicatorRequest>(
                    new StreamReader(controllerBase.Request.Body).ReadToEndAsync().Result);
            }
            catch (JsonException)
            {
                request = default;

                {
                    actionResult = controllerBase.BadRequest("Given request can't be parsed correctly");
                    return false;
                }
            }

            if (request == null)
            {
                actionResult = controllerBase.BadRequest("Given request can't be parsed correctly");
                return false;
            }

            actionResult = null;
            return true;
        }

        private static bool HeadersValid([NotNull] IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
                throw new ArgumentNullException(nameof(headerDictionary));

            var contentType = headerDictionary[HeaderNames.ContentType][0];

            return contentType.Contains(Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase) &&
                   contentType.Contains(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase);
        }
    }
}