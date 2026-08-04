using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Exceptions;
using Pds.FundingClaim.DataProcessor.Services.Extensions;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IReconciliationService"/>
    public class ReconciliationService : IReconciliationService
    {
        private readonly IFundingClaimApiService _fundingClaimApiService;
        private readonly IReconciliationDataService _reconciliationDataService;
        private readonly ILoggerAdapter<ReconciliationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationService"/> class.
        /// </summary>
        /// <param name="fundingClaimApiService">The Funding Claim Api Service.</param>
        /// <param name="reconciliationDataService">The Reconciliation Data service.</param>
        /// <param name="logger">The logger.</param>
        public ReconciliationService(
            IFundingClaimApiService fundingClaimApiService,
            IReconciliationDataService reconciliationDataService,
            ILoggerAdapter<ReconciliationService> logger)
        {
            _fundingClaimApiService = fundingClaimApiService;
            _reconciliationDataService = reconciliationDataService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ReadAndProcessReconciliationFeed()
        {
            var bookmarkId = await GetReconciliationFeedBookmarkIdSetting();
            var bookmark = bookmarkId.ToGuid();

            try
            {
                var reconciliationsToProcess = await _reconciliationDataService.GetReconciliationsToProcess(bookmarkId);
                _logger.LogInformation($"ReadAndProcessReconciliationFeed: Number of reconciliations found to process [{reconciliationsToProcess.Count}]");

                foreach (FeedReconciliation reconcilation in reconciliationsToProcess)
                {
                    _logger.LogInformation($"ReadAndProcessReconciliationFeed: Calling CreateReconciliation for id [{reconcilation.FeedId}]");
                    await _fundingClaimApiService.CreateReconciliation(reconcilation);
                    _logger.LogInformation($"ReadAndProcessReconciliationFeed: CreateReconciliation completed for id [{reconcilation.FeedId}]");
                    _logger.LogInformation($"ReadAndProcessReconciliationFeed: Updating latest reconcilation feed bookmark id to [{reconcilation.FeedId}]");
                    await _fundingClaimApiService.UpdateReconciliationFeedBookmarkId(reconcilation.FeedId);
                }
            }
            catch (BaseFeedReadException exception)
            {
                _logger.LogError($"Exception occured whilst reading Reconciliation feed with details : {exception.Message}");

                await _fundingClaimApiService.AuditReconciliationFeedReadException(exception.Message);

                var exceptionType = FeedReadExceptionMessage.ExceptionType.BookmarkNotMatched;

                if (exception is EmptyPageOnFeedException)
                {
                    exceptionType = FeedReadExceptionMessage.ExceptionType.EmptyPageOnFeed;
                }

                await SendFeedReadExceptionEmail(exceptionType, bookmark, exception.Url);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception occurred while creating reconciliation with details: {exception.Message}");
            }
        }

        /// <summary>
        /// Gets reconciliation feed bookmark id setting.
        /// </summary>
        /// <returns>Retuns reconciliation feed bookmark id setting.</returns>
        private async Task<string> GetReconciliationFeedBookmarkIdSetting()
        {
            var reconciliationFeedBookmarkIdSetting = await _fundingClaimApiService.GetReconciliationFeedBookmarkIdSetting();

            return reconciliationFeedBookmarkIdSetting;
        }

        /// <summary>
        /// Creates the message to send an email as a feed bookmark exception has occurred.
        /// </summary>
        /// <param name="exceptionType">The type of exception that was thrown.</param>
        /// <param name="bookmark">The bookmark that was not matched.</param>
        /// <param name="url">The URL that was being used at the time.</param>
        private async Task SendFeedReadExceptionEmail(FeedReadExceptionMessage.ExceptionType exceptionType, Guid bookmark, string url)
        {
            var message = new FeedReadExceptionMessage
            {
                Type = exceptionType,
                Bookmark = bookmark,
                Url = url
            };

            await _fundingClaimApiService.SendFeedReadExceptionEmail(message);
        }
    }
}