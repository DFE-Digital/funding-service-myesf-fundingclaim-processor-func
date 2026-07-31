using Pds.FundingClaim.CorporateSchema.FundingClaims;
using CorporateFundingClaim = Pds.FundingClaim.CorporateSchema.FundingClaims.FundingClaim;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Service to call Data Collection Funding claim API.
    /// </summary>
    public interface IFundingClaimDataService
    {
        /// <summary>
        /// Gets the funding claim window details from data collection api.
        /// </summary>
        /// <returns>The list of funding claim window details.</returns>
        Task<IEnumerable<FundingClaimDetails>> GetFundingClaimWindowDetails();

        /// <summary>
        /// Gets the funding claims from data collection api.
        /// </summary>
        /// <param name="sinceDateTime">The time after which the funding claim submissions are to be retrieved.</param>
        /// <param name="requireSignature">Whether the funding claims to be retrieved requires to be signed or not.</param>
        /// <returns>The enumeration of funding claims matching the filter.</returns>
        Task<IEnumerable<CorporateFundingClaim>> GetFundingClaim(DateTime sinceDateTime, bool requireSignature);
    }
}