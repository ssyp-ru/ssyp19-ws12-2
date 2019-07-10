using System;
using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ssyp.Communicator.Api
{
    internal static class Responses
    {
        internal static ContentResult CreateContent([NotNull] this object response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            return new ContentResult
                {Content = JsonConvert.SerializeObject(response), ContentType = MediaTypeNames.Application.Json};
        }
    }
}