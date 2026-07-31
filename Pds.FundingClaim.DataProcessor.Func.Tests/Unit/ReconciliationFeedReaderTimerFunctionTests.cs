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
    public class ReconciliationFeedReaderTimerFunctionTests
    {
        [TestMethod, TestCategory("Unit")]
        public void Run_DoesNotThrowException()
        {
            // Arrange
            var mockReconciliationService = new Mock<IReconciliationService>(MockBehavior.Strict);

            mockReconciliationService
                .Setup(service => service.ReadAndProcessReconciliationFeed())
                .Returns(Task.CompletedTask);

            var date = new DateTime(2021, 06, 25, 17, 26, 08);
            var mockSystemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);
            mockSystemProvider.Setup(
                    msp => msp.Now())
                .Returns(date);

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationFeedReaderTimerFunction>>();

            var function = new ReconciliationFeedReaderTimerFunction(
                mockReconciliationService.Object, mockSystemProvider.Object, mockLogger.Object);

            // Act
            Func<Task> act = async () => { await function.Run(null); };

            // Assert
            act.Should().NotThrowAsync();
            mockReconciliationService.Verify(
                mcs => mcs.ReadAndProcessReconciliationFeed(),
                Times.Once);

            mockLogger.Verify(
                l => l.LogInformation(
                    $"ReconciliationFeedReader Timer trigger function executed at: {date}."),
                Times.Once);
        }
    }
}