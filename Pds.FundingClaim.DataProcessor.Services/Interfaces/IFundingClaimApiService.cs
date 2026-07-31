using Pds.FundingClaim.CorporateSchema.FundingClaims;
using Pds.FundingClaim.DataProcessor.Services.Models;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Service to call funding claim internal api.
    /// </summary>
    public interface IFundingClaimApiService
    {
        /// <summary>
        /// Gets the funding claim last retrieved setting.
        /// </summary>
        /// <returns>The funding claim last retrieved setting.</returns>
        Task<string> GetFundingClaimLastRetrievedSetting();

        /// <summary>
        /// Gets the funding claim polling setting.
        /// </summary>
        /// <returns>The funding claim polling setting.</returns>
        Task<string> GetFundingClaimPollingSetting();

        /// <summary>
        /// Gets the use json format of funding claims setting.
        /// </summary>
        /// <returns>The funding claim polling setting.</returns>
        Task<string> GetUseJsonFormatOfFundingClaimsSetting();

        /// <summary>
        /// Calls funding claim internal api to update the funding claim window data.
        /// </summary>
        /// <param name="fundingClaimDetails">The funding claim window details.</param>
        /// <returns>The asynchronous Task.</returns>
        Task UpdateFundingClaimWindows(List<FundingClaimDetails> fundingClaimDetails);

        /// <summary>
        /// Gets the funding claim current window.
        /// </summary>
        /// <returns>The funding claim current window if exists.</returns>
        Task<FundingClaimWindow> GetFundingClaimCurrentWindow();

        /// <summary>
        /// Calls funding claim internal api to update the funding claim window.
        /// </summary>
        /// <param name="fundingClaimsRequest">The funding claim submissions request to be sent.</param>
        /// <returns>The asynchronous Task.</returns>
        Task UpdateFundingClaims(CreateFundingClaimsApiRequest fundingClaimsRequest);

        /// <summary>
        /// Calls Funding claim internal api to autowithdraw the funding claims that have passed the signature close date.
        /// </summary>
        /// <returns>The asynchronous Task.</returns>
        Task AutowithdrawFundingClaims();

        /// <summary>
        /// Gets the reconciliation feed bookmark id setting.
        /// </summary>
        /// <returns>The reconciliation feed bookmark id setting.</returns>
        Task<string> GetReconciliationFeedBookmarkIdSetting();

        /// <summary>
        /// Sets the reconciliation feed bookmark id setting.
        /// </summary>
        /// <param name="bookmarkId">The new bookmark id setting.</param>
        /// <returns>The asynchronous task.</returns>
        Task UpdateReconciliationFeedBookmarkId(Guid bookmarkId);

        /// <summary>
        /// Calls funding claim internal api to create reconciliations.
        /// </summary>
        /// <param name="reconciliation">The reconciliation to be created.</param>
        /// <returns>The asynchronous Task.</returns>
        Task CreateReconciliation(FeedReconciliation reconciliation);

        /// <summary>
        /// Calls funding claim api to audit reconciliation feed read exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>The asynchronous Task.</returns>
        Task AuditReconciliationFeedReadException(string message);

        /// <summary>
        /// Calls funding claim api to send a feed read exception email.
        /// </summary>
        /// <param name="message">Message containing exception details.</param>
        /// <returns>The asynchronous Task.</returns>
        Task SendFeedReadExceptionEmail(FeedReadExceptionMessage message);

        /// <summary>
        /// Gets the reconciliation feed read warning threshold setting.
        /// </summary>
        /// <returns>The reconciliation feed read warning threshold setting.</returns>
        Task<string> GetFeedReadWarningThresholdSetting();

        /// <summary>
        /// Calls funding claim api to send feed exceeded read threshold warning email.
        /// </summary>
        /// <param name="message">>Message containing read times details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendFeedExceededReadThresholdWarningEmail(FeedReadThresholdExceededWarningMessage message);
    }
}