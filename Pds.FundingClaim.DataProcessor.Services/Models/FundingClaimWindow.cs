using System;

namespace Pds.FundingClaim.DataProcessor.Services.Models
{
    /// <summary>
    /// Represents a funding claim window.
    /// </summary>
    public class FundingClaimWindow
    {
        /// <summary>
        /// Gets or sets identifier for the funding claim window.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Data Collection Key.
        /// </summary>
        public string DataCollectionKey { get; set; }

        /// <summary>
        /// Gets or sets the Submission Open Date.
        /// </summary>
        public DateTime SubmissionOpenDate { get; set; }

        /// <summary>
        /// Gets or sets the Submission Close Date.
        /// </summary>
        public DateTime SubmissionCloseDate { get; set; }

        /// <summary>
        /// Gets or sets the Signature Close Date.
        /// </summary>
        public DateTime? SignatureCloseDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding claim window requires signature.
        /// </summary>
        public bool RequiresSignature { get; set; }

        /// <summary>
        /// Gets or sets when the instance was last modified.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }
    }
}