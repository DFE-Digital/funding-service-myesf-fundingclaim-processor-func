using CorporateFundingClaim = Pds.FundingClaim.CorporateSchema.FundingClaims.FundingClaim;

namespace Pds.FundingClaim.DataProcessor.Services.Models
{
    /// <summary>
    /// Concrete class that defines the structure of the request passed into
    /// <see cref="Controllers.FundingClaimController.CreateFundingClaims(CreateFundingClaimsApiRequest)"/> action.
    /// </summary>
    public class CreateFundingClaimsApiRequest
    {
        /// <summary>
        /// Gets or sets the list of funding claims from DCT.
        /// </summary>
        /// <value>The list of funding claims.</value>
        public List<CorporateFundingClaim> FundingClaims { get; set; }

        /// <summary>
        /// Gets or sets the funding claim window ID.
        /// </summary>
        /// <value>The last funding claim window Id.</value>
        public int FundingClaimWindowId { get; set; }
    }
}