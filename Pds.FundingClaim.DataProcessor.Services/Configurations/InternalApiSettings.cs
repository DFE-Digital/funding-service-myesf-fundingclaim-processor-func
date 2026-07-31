namespace Pds.FundingClaim.DataProcessor.Services.Configurations
{
    /// <summary>
    /// Configuration settings for funding claim internal api.
    /// </summary>
    public class InternalApiSettings
    {
        /// <summary>
        /// Gets or sets base uri for the internal api.
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// Gets the uri for the funding claim internal api.
        /// </summary>
        public string FundingClaimUri => BaseUri + "api/FundingClaim/";

        /// <summary>
        /// Gets the uri for the reconciliation internal api.
        /// </summary>
        public string ReconciliationUri => BaseUri + "api/Reconciliation/";
    }
}