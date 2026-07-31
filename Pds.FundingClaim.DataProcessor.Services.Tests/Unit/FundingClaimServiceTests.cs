using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.FundingClaims;
using Pds.FundingClaim.DataProcessor.Services.Constants;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorporateFundingClaim = Pds.FundingClaim.CorporateSchema.FundingClaims.FundingClaim;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
    [TestClass]
    public class FundingClaimServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        [DataRow("true", true)]
        [DataRow("FALSE", false)]
        public async Task ShouldUseNewDCAPI_WhenCalled_ChecksUsage(string returnFromApi, bool returnFromService)
        {
            // Arrange
            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>(MockBehavior.Strict);

            mockFundingClaimApiService
                .Setup(service => service.GetUseJsonFormatOfFundingClaimsSetting())
                .ReturnsAsync(returnFromApi);

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, null, null);

            // Act
            var result = await fundingClaimService.ShouldUseNewDCAPI();

            // Assert
            result.Should().Be(returnFromService);
        }

        #region UpdateFundingClaimWindows Tests

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingClaimWindows_WhenFundingClaimPollingSettingIsTrue_DoesFundingClaimWindowUpdates()
        {
            // Arrange
            var fundingClaimDetails = new List<FundingClaimDetails>
            {
                new FundingClaimDetails { DataCollectionKey = "15-16" },
                new FundingClaimDetails { DataCollectionKey = "19-20" }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("true")
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            mockFundingClaimDataService
                .Setup(service => service.GetFundingClaimWindowDetails())
                .ReturnsAsync(fundingClaimDetails)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.UpdateFundingClaimWindows();

            // Assert
            mockFundingClaimApiService.Verify();
            mockFundingClaimDataService.Verify();

            mockFundingClaimApiService.Verify(
                service => service.UpdateFundingClaimWindows(fundingClaimDetails), Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed UpdateFundingClaimWindows."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingClaimWindows_WhenFundingClaimPollingSettingIsNotTrue_DoesNotDoFundingClaimWindowUpdates()
        {
            // Arrange
            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("false")
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.UpdateFundingClaimWindows();

            // Assert
            mockFundingClaimApiService.Verify();

            mockFundingClaimDataService.Verify(service => service.GetFundingClaimWindowDetails(), Times.Never);

            mockFundingClaimApiService.Verify(service => service.UpdateFundingClaimWindows(It.IsAny<List<FundingClaimDetails>>()), Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed UpdateFundingClaimWindows."),
                Times.Never);
        }

        #endregion UpdateFundingClaimWindows Tests


        #region GetFundingClaims Tests

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaims_WhenFundingClaimPollingSettingIsTrueWithCurrentWindow_AndSubmissionOpenDateIsLater_DoesFundingClaimsUpdates()
        {
            // Arrange
            var currentFundingClaimWindow = new FundingClaimWindow { SubmissionOpenDate = new DateTime(2000, 12, 12), RequiresSignature = true };

            var fundingClaims = new List<CorporateFundingClaim>
            {
                new CorporateFundingClaim { FundingClaimId = "1234" },
                new CorporateFundingClaim { FundingClaimId = "5678" }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("true")
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimLastRetrievedSetting())
                .ReturnsAsync(currentFundingClaimWindow.SubmissionOpenDate.AddDays(-5).ToString())
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimCurrentWindow())
                .ReturnsAsync(currentFundingClaimWindow)
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            mockFundingClaimDataService
                .Setup(service => service.GetFundingClaim(currentFundingClaimWindow.SubmissionOpenDate, currentFundingClaimWindow.RequiresSignature))
                .ReturnsAsync(fundingClaims)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.GetFundingClaims();

            // Assert
            mockFundingClaimApiService.Verify();
            mockFundingClaimDataService.Verify();

            mockFundingClaimApiService.Verify(
                service => service.UpdateFundingClaims(It.Is<CreateFundingClaimsApiRequest>(request =>
                request.FundingClaims.Count == fundingClaims.Count
                && request.FundingClaims[0] == fundingClaims[0]
                && request.FundingClaims[1] == fundingClaims[1]
                && request.FundingClaimWindowId == currentFundingClaimWindow.Id)),
                Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{currentFundingClaimWindow.SubmissionOpenDate} that require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{currentFundingClaimWindow.SubmissionOpenDate} that require signature." + Environment.NewLine
                    + "No new funding claims were retrieved."),
                Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaims_WhenFundingClaimPollingSettingIsTrueWithCurrentWindow_ButNoNewClaims_DoesNotUpdateFundingClaims()
        {
            // Arrange
            var currentFundingClaimWindow = new FundingClaimWindow { SubmissionOpenDate = new DateTime(2000, 12, 12), RequiresSignature = true };

            var fundingClaims = new List<CorporateFundingClaim>
            {
                new CorporateFundingClaim { FundingClaimId = "1234" },
                new CorporateFundingClaim { FundingClaimId = "5678" }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("true")
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimLastRetrievedSetting())
                .ReturnsAsync(currentFundingClaimWindow.SubmissionOpenDate.AddDays(-5).ToString())
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimCurrentWindow())
                .ReturnsAsync(currentFundingClaimWindow)
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            mockFundingClaimDataService
                .Setup(service => service.GetFundingClaim(currentFundingClaimWindow.SubmissionOpenDate, currentFundingClaimWindow.RequiresSignature))
                .ReturnsAsync((List<CorporateFundingClaim>)null)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.GetFundingClaims();

            // Assert
            mockFundingClaimApiService.Verify();
            mockFundingClaimDataService.Verify();

            mockFundingClaimApiService.Verify(
                service => service.UpdateFundingClaims(It.IsAny<CreateFundingClaimsApiRequest>()), Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{currentFundingClaimWindow.SubmissionOpenDate} that require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{currentFundingClaimWindow.SubmissionOpenDate} that require signature." + Environment.NewLine
                    + "No new funding claims were retrieved."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaims_WhenFundingClaimPollingSettingIsTrueWithCurrentWindow_AndSubmissionOpenDateIsPrior_DoesFundingClaimsUpdates()
        {
            // Arrange
            var currentFundingClaimWindow = new FundingClaimWindow { SubmissionOpenDate = new DateTime(2000, 12, 12), RequiresSignature = true };
            var lastTimeRetrievedDateTime = currentFundingClaimWindow.SubmissionOpenDate.AddDays(5);

            var fundingClaims = new List<CorporateFundingClaim>
            {
                new CorporateFundingClaim { FundingClaimId = "1234" },
                new CorporateFundingClaim { FundingClaimId = "5678" }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("true")
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimLastRetrievedSetting())
                .ReturnsAsync(lastTimeRetrievedDateTime.ToString())
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimCurrentWindow())
                .ReturnsAsync(currentFundingClaimWindow)
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            var sinceDateTime = lastTimeRetrievedDateTime.AddHours(ServiceConstants.ContigencyPeriodInHours);

            mockFundingClaimDataService
                .Setup(service => service.GetFundingClaim(sinceDateTime, currentFundingClaimWindow.RequiresSignature))
                .ReturnsAsync(fundingClaims)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.GetFundingClaims();

            // Assert
            mockFundingClaimApiService.Verify();
            mockFundingClaimDataService.Verify();

            mockFundingClaimApiService.Verify(
                service => service.UpdateFundingClaims(It.Is<CreateFundingClaimsApiRequest>(request =>
                request.FundingClaims.Count == fundingClaims.Count
                && request.FundingClaims[0] == fundingClaims[0]
                && request.FundingClaims[1] == fundingClaims[1]
                && request.FundingClaimWindowId == currentFundingClaimWindow.Id)),
                Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{sinceDateTime} that require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaims_WhenFundingClaimPollingSettingIsTrueWithNoCurrentWindow_DoesNotUpdateFundingClaims()
        {
            // Arrange
            var fundingClaims = new List<CorporateFundingClaim>
            {
                new CorporateFundingClaim { FundingClaimId = "1234" },
                new CorporateFundingClaim { FundingClaimId = "5678" }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("true")
                .Verifiable();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimCurrentWindow())
                .ReturnsAsync((FundingClaimWindow)null)
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.GetFundingClaims();

            // Assert
            mockFundingClaimApiService.Verify();
            mockFundingClaimDataService.Verify();

            mockFundingClaimApiService.Verify(
                service => service.UpdateFundingClaims(It.IsAny<CreateFundingClaimsApiRequest>()), Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{It.IsAny<DateTime>()} that require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{It.IsAny<DateTime>()} that do not require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaims_WhenFundingClaimPollingSettingIsNotTrue_DoesNotUpdateFundingClaims()
        {
            // Arrange
            var fundingClaims = new List<CorporateFundingClaim>
            {
                new CorporateFundingClaim { FundingClaimId = "1234" },
                new CorporateFundingClaim { FundingClaimId = "5678" }
            };

            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.GetFundingClaimPollingSetting())
                .ReturnsAsync("false")
                .Verifiable();

            var mockFundingClaimDataService = new Mock<IFundingClaimDataService>();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, mockFundingClaimDataService.Object, mockLogger.Object);

            // Act
            await fundingClaimService.GetFundingClaims();

            // Assert
            mockFundingClaimApiService.Verify();

            mockFundingClaimApiService.Verify(
                service => service.GetFundingClaimCurrentWindow(), Times.Never);

            mockFundingClaimApiService.Verify(
                service => service.UpdateFundingClaims(It.IsAny<CreateFundingClaimsApiRequest>()), Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{It.IsAny<DateTime>()} that require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Never);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimService successfully executed GetFundingClaims for claims created since "
                    + $"{It.IsAny<DateTime>()} that do not require signature." + Environment.NewLine
                    + "Funding claims were retrieved."),
                Times.Never);
        }

        #endregion GetFundingClaims Tests


        [TestMethod, TestCategory("Unit")]
        public async Task AutowithdrawFundingClaims_WhenCalled_CallsApiServiceToAutowithdraw()
        {
            // Arrange
            var mockFundingClaimApiService = new Mock<IFundingClaimApiService>();

            mockFundingClaimApiService
                .Setup(service => service.AutowithdrawFundingClaims())
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimService>>();

            var fundingClaimService = new FundingClaimService(mockFundingClaimApiService.Object, null, mockLogger.Object);

            // Act
            await fundingClaimService.AutowithdrawFundingClaims();

            // Assert
            mockFundingClaimApiService.Verify();
            mockLogger.Verify(
                l => l.LogInformation(
                        "FundingClaimService successfully executed AutowithdrawFundingClaims."),
                Times.Once);
        }
    }
}