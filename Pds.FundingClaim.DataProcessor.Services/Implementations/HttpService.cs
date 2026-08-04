using Microsoft.Net.Http.Headers;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System.Collections.Specialized;
using System.Diagnostics;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Xml.Linq;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IHttpService"/>
    public class HttpService : IHttpService
    {
        private readonly HttpClient _apiClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggerAdapter<HttpService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// </summary>
        /// <param name="authenticationService">The service to get oauth access token.</param>
        /// <param name="apiClient">The http client.</param>
        /// <param name="logger">The logger.</param>
        public HttpService(IAuthenticationService authenticationService, HttpClient apiClient, ILoggerAdapter<HttpService> logger)
        {
            _apiClient = apiClient;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string> GetFromFundingClaimAPIWithMSILAuthenticationAsync(string url, NameValueCollection parameters = null)
        {
            var accessToken = await GetMSILAccessTokenForFundingClaimAPI();
            var content = await GetContent(url, accessToken, parameters);

            return content;
        }

        /// <inheritdoc/>
        public async Task<string> GetFromDCWithMSILAuthenticationAsync(string url, NameValueCollection parameters = null)
        {
            var accessToken = await _authenticationService.GetAccessTokenForDCMSIL().ConfigureAwait(false);
            var content = await GetContent(url, accessToken, parameters);

            return content;
        }

        /// <inheritdoc/>
        public async Task PostWithMSILAuthenticationAsync(string url, object entities)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Post, url);

            var data = JsonSerializer.Serialize(entities);
            httpRequestMsg.Content = new StringContent(data, Encoding.UTF8, "application/json");

            await SendContent(httpRequestMsg);
        }

        /// <inheritdoc/>
        public async Task PutWithMSILAuthenticationAsync(string url, object entities = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Put, url);

            await SendContent(httpRequestMsg);
        }

        /// <inheritdoc/>
        public async Task<SyndicationFeed> GetAtomSyndicationFeed(string uri, Action<string> afterEachPageLoad)
        {
            var accessToken = await _authenticationService.GetAccessTokenForFCSMSIL().ConfigureAwait(false);
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Get, uri);

            httpRequestMsg.Headers.Add("Accept", "application/atom+xml");
            httpRequestMsg.Headers.Add(HeaderNames.Authorization, $"Bearer {accessToken}");

            _logger.LogDebug($"About to make call to uri {uri}");

            var sw = Stopwatch.StartNew();
            var response = await _apiClient.SendAsync(httpRequestMsg).ConfigureAwait(false);
            var rawXml = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            afterEachPageLoad?.Invoke(uri);

            sw.Stop();
            _logger.LogDebug($"Load of feed page {uri} took {sw.ElapsedMilliseconds}ms");

            var formatter = new Atom10FeedFormatter();
            var doc = XDocument.Parse(rawXml);
            using (var reader = doc.CreateReader())
            {
                formatter.ReadFrom(reader);
            }

            _logger.LogDebug($"The load of the feed for uri {uri} contained {formatter.Feed.Items.Count()} items");
            return formatter.Feed;
        }

        #region Private Methods

        /// <summary>
        /// Gets the MSIL auth token from authentication service for Funding Claim API.
        /// </summary>
        /// <returns>The access token.</returns>
        private async Task<string> GetMSILAccessTokenForFundingClaimAPI()
        {
            var accessToken = await _authenticationService.GetAccessTokenForFundingClaimMSIL().ConfigureAwait(false);
            return accessToken;
        }

        /// <summary>
        /// Gets the content from http get call with oauth.
        /// </summary>
        /// <param name="url">The url to be called.</param>
        /// <param name="accessToken">The authentication access token.</param>
        /// <param name="parameters">The query string parameters.</param>
        /// <returns>The content from rest api call.</returns>
        private async Task<string> GetContent(string url, string accessToken, NameValueCollection parameters = null)
        {
            if (parameters != null)
            {
                url = url + ToQueryString(parameters);
            }

            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Get, url);
            SetRequestHeaders(httpRequestMsg, accessToken);
            string contentLength = null;

            try
            {
                var response = await _apiClient.SendAsync(httpRequestMsg).ConfigureAwait(false);

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                contentLength = (content == null) ? "null" : content.Length.ToString();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        $"Exception: Unable to get data from API. Uri: {url}, StatusCode: {response.StatusCode}, Response: {response}, Content: {content}");
                    throw new Exception($"Unable to get data from API. Uri: {url}, Response: {response}, Content: {content} ");
                }

                _logger.LogInformation($"HttpService successfully executed GetContent from url {url}. Content length: {contentLength}");

                return content;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: Unable to connect to API. Uri: {url} Error message: {e.Message}. Content length: {contentLength}");
                throw;
            }
        }

        /// <summary>
        /// Sends the content to http call with oauth.
        /// </summary>
        /// <param name="httpRequestMsg">The request to be sent.</param>
        private async Task SendContent(HttpRequestMessage httpRequestMsg)
        {
            var accessToken = await GetMSILAccessTokenForFundingClaimAPI();
            SetRequestHeaders(httpRequestMsg, accessToken);

            var response = await _apiClient.SendAsync(httpRequestMsg).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Exception: Unable to update data in API." + Environment.NewLine
                    + $"  Uri: {httpRequestMsg.RequestUri}" + Environment.NewLine
                    + $"  StatusCode: {response.StatusCode}" + Environment.NewLine
                    + $"  Response: {response}");

                throw new Exception($"Unable to update data in API. Uri: {httpRequestMsg.RequestUri}, Response: {response}");
            }

            _logger.LogInformation($"HttpService successfully executed SendContent to uri {httpRequestMsg.RequestUri}.");
        }

        /// <summary>
        /// Sets the http request message header.
        /// </summary>
        /// <param name="httpRequestMsg">The message to be set up.</param>
        /// <param name="accessToken">The access token.</param>
        private void SetRequestHeaders(HttpRequestMessage httpRequestMsg, string accessToken)
        {
            httpRequestMsg.Headers.Add(HeaderNames.Authorization, $"Bearer {accessToken}");
        }

        /// <summary>
        /// To generates the query strings for the key value pairs.
        /// </summary>
        /// <param name="nameValueCollection">The query parameters key value pairs.</param>
        /// <returns>The formed query string.</returns>
        private string ToQueryString(NameValueCollection nameValueCollection)
        {
            var array = (
                from key in nameValueCollection.AllKeys
                from value in nameValueCollection.GetValues(key)
                select string.Format(
            "{0}={1}",
            HttpUtility.UrlEncode(key),
            HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        #endregion
    }
}