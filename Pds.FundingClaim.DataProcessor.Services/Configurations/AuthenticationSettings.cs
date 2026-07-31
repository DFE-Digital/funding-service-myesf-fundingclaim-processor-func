namespace Pds.FundingClaim.DataProcessor.Services.Configurations
{
    /// <summary>
    /// Includes the base settings for authentication.
    /// </summary>
    public class AuthenticationSettings
    {
        /// <summary>
        /// Gets or sets authority for the api.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets tenant id for the api.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets client id for the api.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets client secret for the api.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}