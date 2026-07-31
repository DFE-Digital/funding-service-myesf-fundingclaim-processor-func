namespace Pds.FundingClaim.DataProcessor.Services.Configurations
{
    /// <summary>
    /// Configuration settings for funding claim data collection api.
    /// </summary>
    public class DataCollectionApiSettings : AuthenticationSettings
    {
        /// <summary>
        /// Gets or sets scope for the api authentication call.
        /// </summary>
        public string Scope { get; set; }
    }
}