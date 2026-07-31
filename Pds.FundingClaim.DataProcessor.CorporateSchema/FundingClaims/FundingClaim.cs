using System;

namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Represents the funding claim submissions.
    /// </summary>
    public class FundingClaim
    {
        /// <summary>
        /// Gets or sets the Funding Claim Identifier.
        /// </summary>
        public string FundingClaimId { get; set; }

        /// <summary>
        /// Gets or sets the Organisation Identifier.
        /// </summary>
        public string OrganisationIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the Ukprn.
        /// </summary>
        public string Ukprn { get; set; }

        /// <summary>
        /// Gets or sets the Version Number.
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the Claim Type Name.
        /// </summary>
        public string ClaimTypeName { get; set; }

        /// <summary>
        /// Gets or sets the Period Type Code.
        /// </summary>
        public string PeriodTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding claim has been Signed.
        /// </summary>
        public bool HasBeenSigned { get; set; }

        /// <summary>
        /// Gets or sets the Submission DateTime.
        /// </summary>
        public DateTime SubmissionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Contract Allocations.
        /// </summary>
        public ContractAllocation[] ContractAllocations { get; set; }
    }
}