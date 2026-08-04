namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Service to do funding claim data related operations.
    /// </summary>
    public interface IFundingClaimService
    {
        /// <summary>
        /// Detemines if new DC API should be used for funding claims or not.
        /// </summary>
        /// <returns>The check result.</returns>
        Task<bool> ShouldUseNewDCAPI();

        /// <summary>
        /// Calls Data Collection Funding claim service to get funding claim windows information and updates Pds.
        /// </summary>
        /// <returns>The asynchronous Task.</returns>
        Task UpdateFundingClaimWindows();

        /// <summary>
        /// Calls Data Collection Funding claim service to get funding claims in current window information and updates Pds.
        /// </summary>
        /// <returns>The asynchronous Task.</returns>
        Task GetFundingClaims();

        /// <summary>
        /// Calls Funding claim internal api service to autowithdraw the funding claims that have passed the signature close date.
        /// </summary>
        /// <returns>The asynchronous Task.</returns>
        Task AutowithdrawFundingClaims();
    }
}