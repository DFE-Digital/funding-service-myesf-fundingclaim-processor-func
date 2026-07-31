using System.Collections.Specialized;
using System.ServiceModel.Syndication;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Service to do Http operations.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Get the content from Funding Claim api call that implements MSIL oauth.
        /// </summary>
        /// <param name="url">The url to request.</param>
        /// <param name="parameters">The parameters on the query string for the call.</param>
        /// <returns>Result from api.</returns>
        Task<string> GetFromFundingClaimAPIWithMSILAuthenticationAsync(string url, NameValueCollection parameters = null);

        /// <summary>
        /// Get the content from Data Collection api call that implements MSIL oauth.
        /// </summary>
        /// <param name="url">The url to request.</param>
        /// <param name="parameters">The parameters on the query string for the call.</param>
        /// <returns>Result from api.</returns>
        Task<string> GetFromDCWithMSILAuthenticationAsync(string url, NameValueCollection parameters = null);

        /// <summary>
        /// Post the content to api that implements MSIL oauth.
        /// </summary>
        /// <param name="url">The url to request.</param>
        /// <param name="entities">The content to post.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PostWithMSILAuthenticationAsync(string url, object entities);

        /// <summary>
        /// Make a put request with the content to api that implements MSIL oauth.
        /// </summary>
        /// <param name="url">The url to request.</param>
        /// <param name="entities">The content to updated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PutWithMSILAuthenticationAsync(string url, object entities = null);

        /// <summary>
        /// Gets a syndication feed from a uri using MSIL.
        /// </summary>
        /// <param name="uri">The uri to call.</param>
        /// <param name="afterEachPageLoad">Action to take after each page load.</param>
        /// <returns>The feed return.</returns>
        Task<SyndicationFeed> GetAtomSyndicationFeed(string uri, Action<string> afterEachPageLoad);
    }
}