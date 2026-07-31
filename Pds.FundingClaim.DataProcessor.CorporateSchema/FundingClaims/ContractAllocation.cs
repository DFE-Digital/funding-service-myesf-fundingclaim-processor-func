namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Represents the Contract Allocations for the funding claim.
    /// </summary>
    public class ContractAllocation
    {
        /// <summary>
        /// Gets or sets the Contract Allocation Number.
        /// </summary>
        public string ContractAllocationNumber { get; set; }

        /// <summary>
        /// Gets or sets the Funding Stream Period Code.
        /// </summary>
        public string FundingStreamPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Contract Value.
        /// </summary>
        public decimal MaximumContractValue { get; set; }

        /// <summary>
        /// Gets or sets the Deliverable Codes.
        /// </summary>
        public DeliverableCode[] DeliverableCodes { get; set; }
    }
}