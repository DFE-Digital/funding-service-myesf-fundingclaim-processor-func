using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Func.Tests.Unit
{
    [TestClass]
    public class AutowithdrawFundingClaimTimerFunctionTests
    {
        [TestMethod, TestCategory("Unit")]
        public void Run_WhenUseNewAPI_DoesNotThrowException()
        {
            // Arrange
            var mockFundingClaimService = new Mock<IFundingClaimService>(MockBehavior.Strict);

            mockFundingClaimService
                .Setup(service => service.ShouldUseNewDCAPI())
                .ReturnsAsync(true);

            mockFundingClaimService
                .Setup(service => service.AutowithdrawFundingClaims())
                .Returns(Task.CompletedTask);

            var date = new DateTime(2021, 06, 25, 17, 26, 08);
            var mockSystemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);
            mockSystemProvider.Setup(
                    msp => msp.Now())
                .Returns(date);

            var mockLogger = new Mock<ILoggerAdapter<AutowithdrawFundingClaimTimerFunction>>();
            mockLogger.Setup(
                 l => l.LogInformation(
                 $"AutowithdrawFundingClaim Timer trigger function executed at: {date}."))
                .Verifiable();

            var function = new AutowithdrawFundingClaimTimerFunction(
                mockFundingClaimService.Object, mockSystemProvider.Object, mockLogger.Object);

            // Act
            Func<Task> act = async () => { await function.Run(null); };

            // Assert
            act.Should().NotThrowAsync();
            mockFundingClaimService.Verify(
                mcs => mcs.AutowithdrawFundingClaims(),
                Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    $"AutowithdrawFundingClaim timer trigger function started at: {date}."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public void Run_WhenNotUseNewAPI_DoesNotThrowException()
        {
            // Arrange
            var mockFundingClaimService = new Mock<IFundingClaimService>(MockBehavior.Strict);

            var date = new DateTime(2021, 06, 25, 17, 26, 08);
            var mockSystemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);
            mockSystemProvider.Setup(
                    msp => msp.Now())
                .Returns(date);

            mockFundingClaimService
                .Setup(service => service.ShouldUseNewDCAPI())
                .ReturnsAsync(false);

            var mockLogger = new Mock<ILoggerAdapter<AutowithdrawFundingClaimTimerFunction>>();

            var function = new AutowithdrawFundingClaimTimerFunction(
                mockFundingClaimService.Object, mockSystemProvider.Object, mockLogger.Object);

            // Act
            Func<Task> act = async () => { await function.Run(null); };

            // Assert
            act.Should().NotThrowAsync();
            mockFundingClaimService.Verify(
                mcs => mcs.AutowithdrawFundingClaims(),
                Times.Never);
        }
    }
}