namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Represents the Funding Claim Window.
    /// </summary>
    public class FundingClaimDetails
    {
        /// <summary>
        /// Gets or sets the key that identifies which Data Collection that this data was related to within DEDS.
        /// </summary>
        public string DataCollectionKey { get; set; }

        /// <summary>
        /// Gets or sets the time at which Providers can begin to enter and submit their final data into DC.
        /// </summary>
        public string SubmissionOpenDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the time at which Providers can no longer enter and submit their final data into DC.
        /// </summary>
        public string SubmissionCloseDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the time at which Providers can no longer sign their funding claims within SFS.
        /// </summary>
        public string SignatureCloseDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the funding claims associated with this window should be signed.
        /// </summary>
        public bool RequiresSignature { get; set; }
    }
}