using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MixerCore.Authentication;
using Newtonsoft.Json;

namespace MixerCore.Rest
{
    public class MixerRestBase : HttpClient
    {
        private readonly AuthInfo authInfo;

        public MixerRestBase(AuthInfo auth = null) : base(CreateWebRequestHandler(), true) => authInfo = auth;
        public MixerRestBase() : base(CreateWebRequestHandler(), true) { }

        /// <summary>
        /// Gets the type of token
        /// </summary>
        protected string AuthenticationScheme => authInfo.TokenType;

        /// <summary>
        /// Gets the OAuth access token
        /// </summary>
        protected string AccessToken => authInfo.AccessToken;

        /// <summary>
        /// Helper method to set the handler settings for this HttpClient
        /// </summary>
        /// <returns>The HttpMessageHandler</returns>
        private static HttpMessageHandler CreateWebRequestHandler()
        {
            var requestHandler = new HttpClientHandler 
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true,
            };

            return requestHandler;
        }

        /// <summary>
        /// Handles a web request and deserializes the response body.
        /// </summary>
        /// <typeparam name="T">The object type to deserialize into.</typeparam>
        /// <param name="response">The completed HTTP response.</param>
        /// <returns>A deserialized object form the response.</returns>
        protected async Task<T> GetResponseAsync<T>(HttpResponseMessage response) 
            where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new WebException(error);
            }

            if (response.StatusCode == HttpStatusCode.NoContent) 
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var contract = JsonConvert.DeserializeObject<T>(content);
            return contract;
        }

    }
}