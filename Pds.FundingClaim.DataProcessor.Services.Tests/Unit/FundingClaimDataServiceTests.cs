using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.FundingClaims;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Constants;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using CorporateFundingClaim = Pds.FundingClaim.CorporateSchema.FundingClaims.FundingClaim;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
    [TestClass]
    public class FundingClaimDataServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimWindowDetails_WhenCalled_ReturnsFundingClaimWindows()
        {
            // Arrange
            var settings = new DataCollectionApiEndpointSettings
            {
                BaseUri = "base uri",
                GetFundingClaimCollectionsDetailsEndpoint = "GetFundingClaimCollectionsDetails"
            };

            var fundingClaimDetails = new List<FundingClaimDetails>();

            var result = JsonConvert.SerializeObject(fundingClaimDetails);

            var configurationService = Options.Create(settings);

            var mockIHttpService = new Mock<IHttpService>();
            mockIHttpService
                .Setup(service => service.GetFromDCWithMSILAuthenticationAsync($"{settings.BaseUri}{settings.GetFundingClaimCollectionsDetailsEndpoint}", null))
                .ReturnsAsync(result)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimDataService>>();

            var fundingClaimDataService = new FundingClaimDataService(mockIHttpService.Object, configurationService, mockLogger.Object);

            // Act
            var response = await fundingClaimDataService.GetFundingClaimWindowDetails();

            // Assert
            mockIHttpService.Verify();
            response.Should().BeEquivalentTo(fundingClaimDetails);

            mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimDataService successfully executed GetFundingClaimWindowDetails for uri {settings.BaseUri}{settings.GetFundingClaimCollectionsDetailsEndpoint}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaim_WhenHttpServiceGetsResult_ReturnsFundingClaimsAndCreatesLog()
        {
            // Arrange
            var sinceDateTime = new DateTime(2000, 12, 12);
            var requireSignature = false;
            var settings = new DataCollectionApiEndpointSettings
            {
                BaseUri = "base uri",
                GetFundingClaimsEndpoint = "GetFundingClaims"
            };

            var fundingClaims = new List<CorporateFundingClaim>();

            var result = JsonConvert.SerializeObject(fundingClaims);

            var configurationService = Options.Create(settings);

            var mockIHttpService = new Mock<IHttpService>();

            mockIHttpService
                .Setup(service => service.GetFromDCWithMSILAuthenticationAsync($"{settings.BaseUri}{settings.GetFundingClaimsEndpoint}", It.Is<NameValueCollection>(nv =>
                    nv.Keys[0] == "sinceDateTime" &&
                    nv[0] == sinceDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") &&
                    nv.Keys[1] == "requireSignature" &&
                    nv[1] == requireSignature.ToString() &&
                    nv.Keys[2] == "pageSize" &&
                    nv[2] == ServiceConstants.PageSizeForDCApiCall.ToString())))
                .ReturnsAsync(result)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimDataService>>();

            var fundingClaimDataService = new FundingClaimDataService(mockIHttpService.Object, configurationService, mockLogger.Object);

            // Act
            var response = await fundingClaimDataService.GetFundingClaim(sinceDateTime, requireSignature);

            // Assert
            mockIHttpService.Verify();
            response.Should().BeEquivalentTo(fundingClaims);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimDataService successfully executed GetFundingClaim to uri "
                    + $"{settings.BaseUri}{settings.GetFundingClaimsEndpoint}." + Environment.NewLine
                    + "It sent the request with the following parameters:" + Environment.NewLine
                    + $"   sinceDateTime: {sinceDateTime:yyyy-MM-ddTHH:mm:ss.fffZ}" + Environment.NewLine
                    + $"   requireSignature: {requireSignature}" + Environment.NewLine
                    + $"   pageSize: {ServiceConstants.PageSizeForDCApiCall}"),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaim_WhenHttpServiceGetsNoResult_DoesNotReturnFundingClaimsAndCreatesLog()
        {
            // Arrange
            var sinceDateTime = new DateTime(2000, 12, 12);
            var requireSignature = false;
            var settings = new DataCollectionApiEndpointSettings
            {
                BaseUri = "base uri",
                GetFundingClaimsEndpoint = "GetFundingClaims"
            };
            var fundingClaims = new List<CorporateFundingClaim>();

            var result = JsonConvert.SerializeObject(fundingClaims);

            var configurationService = Options.Create(settings);

            var mockIHttpService = new Mock<IHttpService>();

            mockIHttpService
                .Setup(service => service.GetFromDCWithMSILAuthenticationAsync($"{settings.BaseUri}{settings.GetFundingClaimsEndpoint}", It.Is<NameValueCollection>(nv =>
                    nv.Keys[0] == "sinceDateTime" &&
                    nv[0] == sinceDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") &&
                    nv.Keys[1] == "requireSignature" &&
                    nv[1] == requireSignature.ToString() &&
                    nv.Keys[2] == "pageSize" &&
                    nv[2] == ServiceConstants.PageSizeForDCApiCall.ToString())))
                .ReturnsAsync(result)
                .Verifiable();

            var mockLogger = new Mock<ILoggerAdapter<FundingClaimDataService>>();

            var fundingClaimDataService = new FundingClaimDataService(mockIHttpService.Object, configurationService, mockLogger.Object);

            // Act
            var response = await fundingClaimDataService.GetFundingClaim(sinceDateTime, requireSignature);

            // Assert
            mockIHttpService.Verify();
            response.Should().BeEquivalentTo(fundingClaims);

            mockLogger.Verify(
                l => l.LogInformation(
                    "FundingClaimDataService successfully executed GetFundingClaim to uri "
                    + $"{settings.BaseUri}{settings.GetFundingClaimsEndpoint}." + Environment.NewLine
                    + "It sent the request with the following parameters:" + Environment.NewLine
                    + $"   sinceDateTime: {sinceDateTime:yyyy-MM-ddTHH:mm:ss.fffZ}" + Environment.NewLine
                    + $"   requireSignature: {requireSignature}" + Environment.NewLine
                    + $"   pageSize: {ServiceConstants.PageSizeForDCApiCall}"),
                Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    $"FundingClaimDataService has returned {fundingClaims.Count} funding claims."),
                Times.Once);
        }
    }
}