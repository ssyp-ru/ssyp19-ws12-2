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
                .PostAsync($"{ApiUriBase}conversation/list", new ConversationListRequest(ApiKey).CreateContent())
                ?.ParseJsonAsync<ConversationListResponse>();

            if (task != null)
                return await task;

            return null;
        }

        [CanBeNull]
        internal static async Task<UserInfoOwnResponse> RequestUserInfoOwn()
        {
            var task = Client
                .PostAsync($"{ApiUriBase}/user/info/own", new UserInfoOwnRequest(ApiKey).CreateContent())
                ?.ParseJsonAsync<UserInfoOwnResponse>();

            if (task != null)
                return await task;

            return null;
        }

        [NotNull]
        internal static async Task<bool> RequestConversionSend([NotNull] string receiver, [NotNull] string message)
        {
            return (await Client
                    .PostAsync(
                        $"{ApiUriBase}/conversation/send",
                        new ConversationSendRequest(ApiKey, receiver, message).CreateContent()))
                .IsSuccessStatusCode;
        }

        internal static async void RequestUserModify([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            await Client.PostAsync($"{ApiUriBase}user/modify", new UserModifyRequest(ApiKey, name).CreateContent());
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