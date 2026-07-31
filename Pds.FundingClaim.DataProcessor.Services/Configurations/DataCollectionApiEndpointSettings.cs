namespace Pds.FundingClaim.DataProcessor.Services.Configurations
{
    /// <summary>
    /// Configuration endpoint settings for funding claim data collection api.
    /// </summary>
    public class DataCollectionApiEndpointSettings
    {
        /// <summary>
        /// Gets or sets base uri for the api.
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// Gets or sets uri to get funding claim collection details.
        /// </summary>
        public string GetFundingClaimCollectionsDetailsEndpoint { get; set; }

        /// <summary>
        /// Gets or sets uri to get funding claim collection details.
        /// </summary>
        public string GetFundingClaimsEndpoint { get; set; }

        /// <summary>
        /// Gets or sets uri to get funding claim collection details.
        /// </summary>
        public string GetFundingClaimsCountEndpoint { get; set; }
    }
}