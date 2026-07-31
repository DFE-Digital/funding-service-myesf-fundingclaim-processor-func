using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.FundingClaims;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
    [TestClass]
    public class FundingClaimApiServiceTests
    {
        private const string _result = "Expected Result";
        private InternalApiSettings _settings = new InternalApiSettings { BaseUri = "base uri" };

        private Mock<ILoggerAdapter<FundingClaimApiService>> _mockLogger;

        private Mock<IHttpService> _mockHttpService;

        private IOptions<InternalApiSettings> _apiSettings;

        private FundingClaimApiService _fundingClaimApiService;

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimLastRetrievedSetting_WhenCalled_ReturnSettings()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}GetFundingClaimLastRetrievedSetting";

            SetUpTests(uri, setUpGet: true);

            // Act
            var response = await _fundingClaimApiService.GetFundingClaimLastRetrievedSetting();

            // Assert
            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);

            response.Should().Be(_result);

            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetFundingClaimLastRetrievedSetting for uri {uri}." + Environment.NewLine
                    + $"Result is {_result}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimPollingSetting_WhenCalled_ReturnSettings()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}GetFundingClaimPollingSetting";

            SetUpTests(uri, setUpGet: true);

            // Act
            var response = await _fundingClaimApiService.GetFundingClaimPollingSetting();

            // Assert
            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);

            response.Should().Be(_result);

            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetFundingClaimPollingSetting for uri {uri}." + Environment.NewLine
                + $"Result is {_result}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetUseJsonFormatOfFundingClaimsSetting_WhenCalled_ReturnSettings()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}GetUseJsonFormatOfFundingClaimsSetting";

            SetUpTests(uri, setUpGet: true);

            // Act
            var response = await _fundingClaimApiService.GetUseJsonFormatOfFundingClaimsSetting();

            // Assert
            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);

            response.Should().Be(_result);

            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetUseJsonFormatOfFundingClaimsSetting for uri {uri}." + Environment.NewLine
                + $"Result is {_result}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingClaimWindows_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}UpdateFundingClaimWindow";

            var fundingClaimDetails = new List<FundingClaimDetails>();

            SetUpTests(uri, setUpPost: true, postObject: fundingClaimDetails);

            // Act
            await _fundingClaimApiService.UpdateFundingClaimWindows(fundingClaimDetails);

            // Assert
            _mockLogger.Verify(
                l => l.LogInformation($"FundingClaimApiService successfully processed {fundingClaimDetails.Count} Funding claim windows. uri {uri}."));

            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, fundingClaimDetails), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingClaims_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}CreateFundingClaims";

            var request = new CreateFundingClaimsApiRequest()
            {
                FundingClaimWindowId = 1
            };

            SetUpTests(uri, setUpPost: true, postObject: request);

            // Act
            await _fundingClaimApiService.UpdateFundingClaims(request);

            // Assert
            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed UpdateFundingClaims for uri {uri}." + Environment.NewLine
                    + $"The CreateFundingClaimsApiRequest was posted for FundingClaimWindow: {request.FundingClaimWindowId}"),
                Times.Once);

            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, request), Times.Once);
        }


        #region GetFundingClaimCurrentWindow Tests

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimCurrentWindow_WhenCalledAndWindowExists_ReturnCurrentWindow()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}GetFundingClaimCurrentWindow";

            var fundingClaimWindow = new FundingClaimWindow
            {
                Id = 1,
                DataCollectionKey = "DataCollectionKey",
                SubmissionOpenDate = new DateTime(2000, 1, 2),
                SubmissionCloseDate = new DateTime(2000, 1, 7),
                RequiresSignature = true,
                LastUpdatedAt = new DateTime(2000, 1, 11)
            };
            var result = JsonConvert.SerializeObject(fundingClaimWindow);

            SetUpTests(uri, setUpGet: true, getResult: result);

            // Act
            var response = await _fundingClaimApiService.GetFundingClaimCurrentWindow();

            // Assert
            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetFundingClaimCurrentWindow for uri {uri}." + Environment.NewLine
                    + $"Retrieved FundingClaimWindow Id {fundingClaimWindow.Id}."),
                Times.Once);

            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);

            response.Should().BeEquivalentTo(fundingClaimWindow);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimCurrentWindow_WhenCalledAndWindowDoesNotExist_ReturnNullWindow()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}GetFundingClaimCurrentWindow";

            SetUpTests(uri);

            _mockHttpService
                .Setup(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null))
                .ReturnsAsync((string)null)
                .Verifiable();

            // Act
            var response = await _fundingClaimApiService.GetFundingClaimCurrentWindow();

            // Assert
            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetFundingClaimCurrentWindow for uri {uri}." + Environment.NewLine
                    + "No window was found."),
                Times.Once);

            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);

            response.Should().Be(null);
        }

        #endregion GetFundingClaimCurrentWindow Tests


        [TestMethod, TestCategory("Unit")]
        public async Task AutowithdrawFundingClaims_WhenCalled_CallsTheAutowithdrawEndpoint()
        {
            // Arrange
            var uri = $"{_settings.FundingClaimUri}AutoWithdrawFundingClaims";
            SetUpTests(uri, setUpPut: true);

            // Act
            await _fundingClaimApiService.AutowithdrawFundingClaims();

            // Assert
            _mockHttpService.Verify(service => service.PutWithMSILAuthenticationAsync(uri, null), Times.Once);

            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed AutowithdrawFundingClaims for uri {uri}"),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationFeedBookmarkIdSetting_WhenCalled_ReturnSettings()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}GetReconciliationFeedBookmarkIdSetting";
            SetUpTests(uri, setUpGet: true);

            // Act
            var response = await _fundingClaimApiService.GetReconciliationFeedBookmarkIdSetting();

            // Assert
            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);
            response.Should().Be(_result);

            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetReconciliationFeedBookmarkIdSetting for uri {uri}." + Environment.NewLine
                + $"Result is {_result}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateReconciliation_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}CreateReconciliation";

            FeedReconciliation reconciliation = new FeedReconciliation();
            SetUpTests(uri, setUpPost: true, postObject: reconciliation);

            // Act
            await _fundingClaimApiService.CreateReconciliation(reconciliation);

            // Assert
            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, reconciliation), Times.Once);

            _mockLogger.Verify(
                    l => l.LogInformation(
                        $"FundingClaimApiService successfully executed CreateReconciliation for uri {uri}."),
                    Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateReconciliationFeedBookmarkId_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}UpdateReconciliationFeedBookmarkId";

            Guid bookmarkId = Guid.NewGuid();
            SetUpTests(uri, setUpPost: true, postObject: bookmarkId);

            // Act
            await _fundingClaimApiService.UpdateReconciliationFeedBookmarkId(bookmarkId);

            // Assert
            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, bookmarkId), Times.Once);

            _mockLogger.Verify(
                    l => l.LogInformation(
                        $"FundingClaimApiService successfully executed UpdateReconciliationFeedBookmarkId for uri {uri}."),
                    Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AuditReconciliationFeedReadException_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}AuditReconciliationFeedReadException";

            var message = "Error message";
            SetUpTests(uri, setUpPost: true, postObject: message);

            // Act
            await _fundingClaimApiService.AuditReconciliationFeedReadException(message);

            // Assert
            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, message), Times.Once);

            _mockLogger.Verify(
                    l => l.LogInformation(
                       $"FundingClaimApiService successfully executed AuditReconciliationFeedReadException for uri {uri} and message {message}."),
                    Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SendFeedReadExceptionEmail_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}SendFeedReadExceptionEmail";

            var message = new FeedReadExceptionMessage
                                {
                                    Bookmark = Guid.NewGuid(),
                                    Type = FeedReadExceptionMessage.ExceptionType.BookmarkNotMatched,
                                    Url = "http://url"
                                };
            SetUpTests(uri, setUpPost: true, postObject: message);

            // Act
            await _fundingClaimApiService.SendFeedReadExceptionEmail(message);

            // Assert
            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, message), Times.Once);

            _mockLogger.Verify(
                    l => l.LogInformation(
                        $"Creating azure service bus message to send email for feed exception. Bookmark[{message.Bookmark}], Url[{message.Url}]"),
                    Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFeedReadWarningThresholdSetting_WhenCalled_ReturnSettings()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}GetFeedReadWarningThresholdSetting";
            SetUpTests(uri, setUpGet: true);

            // Act
            var response = await _fundingClaimApiService.GetFeedReadWarningThresholdSetting();

            // Assert
            _mockHttpService.Verify(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null), Times.Once);
            response.Should().Be(_result);

            _mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimApiService successfully executed GetFeedReadWarningThresholdSetting for uri {uri}." + Environment.NewLine
                + $"Result is {_result}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SendFeedExceededReadThresholdWarningEmail_WhenCalled_PostsTheData()
        {
            // Arrange
            var uri = $"{_settings.ReconciliationUri}SendFeedExceededReadThresholdWarningEmail";

            var message = new FeedReadThresholdExceededWarningMessage
            {
                Start = DateTime.Now.AddMinutes(-10),
                Now = DateTime.Now,
                BookmarkId = Guid.NewGuid(),
                LastPageUrl = "url"
            };
            SetUpTests(uri, setUpPost: true, postObject: message);

            // Act
            await _fundingClaimApiService.SendFeedExceededReadThresholdWarningEmail(message);

            // Assert
            _mockHttpService.Verify(service => service.PostWithMSILAuthenticationAsync(uri, message), Times.Once);

            _mockLogger.Verify(
                    l => l.LogInformation(
                        $"Creating azure service bus message to send email for feed read exceeding threshold. Bookmark[{message.BookmarkId}], Url[{message.LastPageUrl}]"),
                    Times.Once);
        }

        private void SetUpTests(string uri, bool setUpGet = false, bool setUpPost = false, bool setUpPut = false, string getResult = null, object postObject = null)
        {
            _apiSettings = Options.Create(_settings);

            _mockHttpService = new Mock<IHttpService>();
            if (setUpGet)
            {
                var result = getResult ?? _result;
                _mockHttpService
                    .Setup(service => service.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri, null))
                    .ReturnsAsync(result)
                    .Verifiable();
            }

            if (setUpPost)
            {
                _mockHttpService
                    .Setup(service => service.PostWithMSILAuthenticationAsync(uri, postObject))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
            }

            if (setUpPut)
            {
                _mockHttpService
                    .Setup(service => service.PutWithMSILAuthenticationAsync(uri, null))
                    .Verifiable();
            }

            _mockLogger = new Mock<ILoggerAdapter<FundingClaimApiService>>();

            _fundingClaimApiService = new FundingClaimApiService(_mockHttpService.Object, _apiSettings, _mockLogger.Object);
        }
    }
}