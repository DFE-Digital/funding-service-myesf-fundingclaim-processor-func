using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Exceptions;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
    [TestClass]
    public class ReconciliationServiceTests
    {
        private Guid _bookmarkId = Guid.NewGuid();
        private string _url = "Feed url";
        private List<FeedReconciliation> _reconciliations = new List<FeedReconciliation>();

        private Mock<IFundingClaimApiService> _mockFundingClaimApiService;
        private Mock<IReconciliationDataService> _mockReconciliationDataService;
        private Mock<ILoggerAdapter<ReconciliationService>> _mockLogger;
        private ReconciliationService _service;

        [TestMethod, TestCategory("Unit")]
        public void ReadAndProcessReconciliationFeed_WhenFeedReadThrowsException_LogsAndAudits()
        {
            // Arrange
            SetUpTests(true, 0, FeedReadExceptionMessage.ExceptionType.BookmarkNotMatched);
            var errorMessage = $"The bookmark [{_bookmarkId}] was not matched and we have reached the beginning of the feed.";

            // Assert
            _ = Assert.ThrowsExceptionAsync<BaseFeedReadException>(async () => await _service.ReadAndProcessReconciliationFeed());

            _mockFundingClaimApiService.Verify(service => service.GetReconciliationFeedBookmarkIdSetting(), Times.Once);

            _mockReconciliationDataService.Verify(service => service.GetReconciliationsToProcess(_bookmarkId.ToString()), Times.Once);

            _mockLogger.Verify(logger => logger.LogError($"Exception occured whilst reading Reconciliation feed with details : {errorMessage}"), Times.Once);

            _mockFundingClaimApiService.Verify(service => service.AuditReconciliationFeedReadException(errorMessage), Times.Once);

            _mockFundingClaimApiService.Verify(service => service.SendFeedReadExceptionEmail(It.Is<FeedReadExceptionMessage>(message => message.Type == FeedReadExceptionMessage.ExceptionType.BookmarkNotMatched)), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadAndProcessReconciliationFeed_WhenFeedReadThrowsEmptyPageOnFeedException_LogsAndAudits()
        {
            // Arrange
            SetUpTests(true, 0, FeedReadExceptionMessage.ExceptionType.EmptyPageOnFeed);
            var errorMessage = $"An empty page was found in the ATOM feed at page [{_url}].";

            // Assert
            _ = Assert.ThrowsExceptionAsync<EmptyPageOnFeedException>(async () => await _service.ReadAndProcessReconciliationFeed());

            _mockFundingClaimApiService.Verify(service => service.GetReconciliationFeedBookmarkIdSetting(), Times.Once);

            _mockReconciliationDataService.Verify(service => service.GetReconciliationsToProcess(_bookmarkId.ToString()), Times.Once);

            _mockLogger.Verify(logger => logger.LogError($"Exception occured whilst reading Reconciliation feed with details : {errorMessage}"), Times.Once);

            _mockFundingClaimApiService.Verify(service => service.AuditReconciliationFeedReadException(errorMessage), Times.Once);

            _mockFundingClaimApiService.Verify(service => service.SendFeedReadExceptionEmail(It.Is<FeedReadExceptionMessage>(message => message.Type == FeedReadExceptionMessage.ExceptionType.EmptyPageOnFeed)), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task ReadAndProcessReconciliationFeed_WhenCalled_CreateReconciliations(int numberOfReconciliations)
        {
            // Arrange
            SetUpTests(false, numberOfReconciliations);

            // Act
            await _service.ReadAndProcessReconciliationFeed();

            // Assert
            _mockFundingClaimApiService.Verify(service => service.GetReconciliationFeedBookmarkIdSetting(), Times.Once);

            _mockReconciliationDataService.Verify(service => service.GetReconciliationsToProcess(_bookmarkId.ToString()), Times.Once);

            _mockFundingClaimApiService.Verify(
                service => service.CreateReconciliation(It.IsAny<FeedReconciliation>()), Times.Exactly(numberOfReconciliations));

            _mockFundingClaimApiService.Verify(service => service.UpdateReconciliationFeedBookmarkId(It.IsAny<Guid>()), Times.Exactly(numberOfReconciliations));
        }

        private void SetUpTests(bool serviceThrowsException, int numberOfReconciliations = 0, FeedReadExceptionMessage.ExceptionType? exceptionType = null)
        {
            _mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            _mockFundingClaimApiService.Setup(service => service.GetReconciliationFeedBookmarkIdSetting())
                .ReturnsAsync(_bookmarkId.ToString());

            _mockReconciliationDataService = new Mock<IReconciliationDataService>();

            _mockLogger = new Mock<ILoggerAdapter<ReconciliationService>>();

            for (int i = 0; i < numberOfReconciliations; i++)
            {
                _reconciliations.Add(new FeedReconciliation());
            }

            if (serviceThrowsException)
            {
                BaseFeedReadException exception;
                if (exceptionType == FeedReadExceptionMessage.ExceptionType.EmptyPageOnFeed)
                {
                    exception = new EmptyPageOnFeedException(_url);
                }
                else
                {
                    exception = new GuidBookmarkNotMatchedException(_bookmarkId, _url);
                }

                _mockReconciliationDataService.Setup(service => service.GetReconciliationsToProcess(_bookmarkId.ToString()))
                .Throws(exception);

                _mockFundingClaimApiService.Setup(service => service.AuditReconciliationFeedReadException(exception.Message))
                    .Returns(Task.CompletedTask);
            }
            else
            {
                _mockReconciliationDataService.Setup(service => service.GetReconciliationsToProcess(_bookmarkId.ToString()))
               .ReturnsAsync(_reconciliations);
            }

            _service = new ReconciliationService(_mockFundingClaimApiService.Object, _mockReconciliationDataService.Object, _mockLogger.Object);
        }
    }
}