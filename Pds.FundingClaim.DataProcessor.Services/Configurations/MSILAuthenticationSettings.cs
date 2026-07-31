namespace Pds.FundingClaim.DataProcessor.Services.Configurations
{
    /// <summary>
    /// Configuration settings for MSIL authentication.
    /// </summary>
    public class MSILAuthenticationSettings : AuthenticationSettings
    {
        /// <summary>
        /// Gets or sets application uri for the api.
        /// </summary>
        public string AppIdUri { get; set; }
    }
}