using Microsoft.Extensions.Options;
using Pds.FundingClaim.CorporateSchema.Reconciliations;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Extensions;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IReconciliationDataService"/>
    public class ReconciliationDataService : IReconciliationDataService
    {
        private readonly FCSApiEndpointSettings _apiSettings;
        private readonly IFeedProcessor _feedProcessor;
        private readonly IFundingClaimApiService _fundingClaimApiService;
        private readonly ISystemProvider _systemProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationDataService"/> class.
        /// </summary>
        /// <param name="feedProcessor">The atom feed processor.</param>
        /// <param name="fundingClaimApiService">The funding claim api service.</param>
        /// <param name="systemProvider">The system provider.</param>
        /// <param name="apiSettings">The api settings.</param>
        public ReconciliationDataService(
            IFeedProcessor feedProcessor,
            IFundingClaimApiService fundingClaimApiService,
            ISystemProvider systemProvider,
            IOptions<FCSApiEndpointSettings> apiSettings)
        {
            _feedProcessor = feedProcessor;
            _fundingClaimApiService = fundingClaimApiService;
            _systemProvider = systemProvider;
            _apiSettings = apiSettings.Value;
        }

        /// <inheritdoc />
        public async Task<IList<FeedReconciliation>> GetReconciliationsToProcess(string bookmarkId)
        {
            var originalBookmark = bookmarkId.ToGuid();
            var start = _systemProvider.Now();
            var feedReadWarningThreshold = await _fundingClaimApiService.GetFeedReadWarningThresholdSetting();
            var warningTime = start + feedReadWarningThreshold.ToTimeSpan();
            var warningMessageSent = false;

            Action<string> afterEachPageLoad = async lastReadUrl =>
            {
                if (!warningMessageSent)
                {
                    var now = _systemProvider.Now();
                    if (now >= warningTime)
                    {
                        warningMessageSent = true;
                        var message = new FeedReadThresholdExceededWarningMessage
                        {
                            Start = start,
                            Now = now,
                            BookmarkId = originalBookmark,
                            LastPageUrl = lastReadUrl
                        };
                        await _fundingClaimApiService.SendFeedExceededReadThresholdWarningEmail(message);
                    }
                }
            };

            var result = await _feedProcessor.ReadAndProcessAfterMatch(
                $"{_apiSettings.BaseUri}/api/performance-management/funding-claim-reconciliations/notifications",
                originalBookmark,
                item => SyndicationContentToInstance<FCReconciliation, FeedReconciliation>(item.Content, item.IdAsGuid(), FeedReconciliation.NewInstance),
                afterEachPageLoad);

            return result;
        }

        private TOut SyndicationContentToInstance<TIn, TOut>(SyndicationContent content, Guid eventId, Func<TIn, Guid, TOut> newInstance)
         where TOut : class
        {
            var xmlContent = content as XmlSyndicationContent;
            if (xmlContent != null)
            {
                return newInstance(xmlContent.Deserialize<TIn>(), eventId);
            }

            return null;
        }
    }
}