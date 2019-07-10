using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Responses;

namespace Ssyp.Communicator.Cli
{
    internal static class Requests
    {
        [NotNull] private static HttpClient Client { get; } = new HttpClient();

        [NotNull] internal static string ApiKey { get; set; } = Guid.Empty.ToString();

        [NotNull] private static string ApiUriBase { get; } = "http://localhost:5000/";

        [CanBeNull]
        internal static async Task<ConversationListResponse> RequestConversationList()
        {
            var task = Client
                .PostAsync($"{ApiUriBase} ", new ConversationListRequest(ApiKey).CreateContent())
                ?.ParseJsonAsync<ConversationListResponse>();

            if (task != null)
                return await task;

            return null;
        }

        [CanBeNull]
        internal static async Task<string> RequestUserName([NotNull] string userID)
        {
            if (userID == null)
                throw new ArgumentNullException(nameof(userID));

            var task = RequestUserInfo(userID);
            return task != null ? (await task).Name : null;
        }

        [CanBeNull]
        internal static async Task<UserInfoResponse> RequestUserInfo([NotNull] string userID)
        {
            if (userID == null)
                throw new ArgumentNullException(nameof(userID));

            var task = Client
                .PostAsync($"{ApiUriBase} ", new UserInfoRequest(ApiKey, userID).CreateContent())
                ?.ParseJsonAsync<UserInfoResponse>();

            if (task != null)
                return await task;

            return null;
        }

        [NotNull]
        private static StringContent CreateContent([NotNull] this ICommunicatorRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }

        [CanBeNull]
        private static async Task<TResponse> ParseJsonAsync<TResponse>(this Task<HttpResponseMessage> responseTask)
            where TResponse : class
        {
            var response = await responseTask;

            return !response.IsSuccessStatusCode
                ? null
                : JsonConvert.DeserializeObject<TResponse>(response.Content.ReadAsStringAsync().Result);
        }
    }
}