using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Ssyp.Communicator.Common.Requests;
using Ssyp.Communicator.Common.Responses;

namespace Ssyp.Communicator.CommonClient
{
    public static class Requests
    {
        [NotNull] private static HttpClient Client { get; } = new HttpClient();

        [NotNull] public static string ApiKey { private get; set; } = "12D5A4A4-F225-4245-A2E5-EA76AB042712";

        [NotNull] private static string ApiUriBase { get; } = "http://localhost:5000/";

        [CanBeNull]
        public static async Task<ConversationListResponse> RequestConversationList()
        {
            var task = Client
                .PostAsync($"{ApiUriBase}conversation/list", new ConversationListRequest(ApiKey).CreateContent())
                ?.ParseJsonAsync<ConversationListResponse>();

            if (task != null)
                return await task;

            return null;
        }

        [CanBeNull]
        public static async Task<UserInfoOwnResponse> RequestUserInfoOwn()
        {
            var task = Client
                .PostAsync($"{ApiUriBase}user/info/own", new UserInfoOwnRequest(ApiKey).CreateContent())
                ?.ParseJsonAsync<UserInfoOwnResponse>();

            if (task != null)
                return await task;

            return null;
        }

        [NotNull]
        public static async Task<bool> RequestConversationSend([NotNull] string receiver, [NotNull] string message)
        {
            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver));

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            return (await Client
                    .PostAsync(
                        $"{ApiUriBase}conversation/send",
                        new ConversationSendRequest(ApiKey, message, receiver).CreateContent()))
                .IsSuccessStatusCode;
        }

        [NotNull]
        public static async Task<bool> RequestUserModify([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (await Client.PostAsync($"{ApiUriBase}user/modify",
                new UserModifyRequest(ApiKey, name).CreateContent())).IsSuccessStatusCode;
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
                : JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}