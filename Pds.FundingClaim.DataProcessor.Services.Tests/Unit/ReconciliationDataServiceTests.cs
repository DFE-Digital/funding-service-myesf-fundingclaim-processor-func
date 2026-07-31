using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.FundingClaim.CorporateSchema.Reconciliations;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
    [TestClass]
    public class ReconciliationDataServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationsToProcess_WhenCalled_ReadsAndReturnsReadFeed()
        {
            // Arrange
            var settings = new FCSApiEndpointSettings { BaseUri = "base uri" };
            IOptions<FCSApiEndpointSettings> apiSettings = Options.Create(settings);

            var bookmarkId = Guid.NewGuid();
            var now = new DateTime(2000, 1, 12);
            var timespan = "00:30:00";

            var result = new List<FeedReconciliation>
            {
                new FeedReconciliation { FeedId = Guid.NewGuid(), Reconciliation = new FCReconciliation() },
                new FeedReconciliation { FeedId = Guid.NewGuid(), Reconciliation = new FCReconciliation() }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>(MockBehavior.Strict);
            mockFundingClaimApiService.Setup(service => service.GetFeedReadWarningThresholdSetting()).ReturnsAsync(timespan);

            var mockSystemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);
            mockSystemProvider.Setup(service => service.Now()).Returns(now);

            var mockFeedProcessor = new Mock<IFeedProcessor>(MockBehavior.Strict);
            mockFeedProcessor.Setup(service => service.ReadAndProcessAfterMatch(
                $"{settings.BaseUri}/api/performance-management/funding-claim-reconciliations/notifications",
                bookmarkId,
                It.IsAny<Func<SyndicationItem, FeedReconciliation>>(),
                It.IsAny<Action<string>>()))
                .ReturnsAsync(result);

            var service = new ReconciliationDataService(mockFeedProcessor.Object, mockFundingClaimApiService.Object, mockSystemProvider.Object, apiSettings);

            // Act
            var response = await service.GetReconciliationsToProcess(bookmarkId.ToString());

            // Assert
            response.Should().BeEquivalentTo(result);
        }
    }
}