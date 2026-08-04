namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Service to do reconciliation data related operations.
    /// </summary>
    public interface IReconciliationService
    {
        /// <summary>
        /// Reads FCS reconciliation feed and calls API to update the results in database.
        /// </summary>
        /// <returns>The asynchronous Task.</returns>
        Task ReadAndProcessReconciliationFeed();
    }
}