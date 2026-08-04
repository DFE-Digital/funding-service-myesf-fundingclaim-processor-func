using Pds.FundingClaim.DataProcessor.Services.Models;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Service to call FCS Reconciliation Feed.
    /// </summary>
    public interface IReconciliationDataService
    {
        /// <summary>
        /// Gets the reconciliations to process since the feed bookmarkid.
        /// </summary>
        /// <returns>The list of feed reconciliations to be processed.</returns>
        /// <param name="bookmarkId">The last read bookmard id.</param>
        Task<IList<FeedReconciliation>> GetReconciliationsToProcess(string bookmarkId);
    }
}