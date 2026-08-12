using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.CorporateSchema.Reconciliations;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Models
{
    [TestClass]
    public class FeedReconciliationTests
    {
        [TestMethod]
        public void NewInstance_Should_Return_FeedReconciliation_Instance()
        {
            // Arrange
            var reconciliation = new FCReconciliation();

            // Act
            var result = FeedReconciliation.NewInstance(
                reconciliation,
                Guid.NewGuid());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FeedReconciliation));
        }

        [TestMethod]
        public void NewInstance_Should_Set_FeedId()
        {
            // Arrange
            var reconciliation = new FCReconciliation();
            var feedId = Guid.NewGuid();

            // Act
            var result = FeedReconciliation.NewInstance(
                reconciliation,
                feedId);

            // Assert
            Assert.AreEqual(feedId, result.FeedId);
        }

        [TestMethod]
        public void NewInstance_Should_Set_Reconciliation()
        {
            // Arrange
            var reconciliation = new FCReconciliation();

            // Act
            var result = FeedReconciliation.NewInstance(
                reconciliation,
                Guid.NewGuid());

            // Assert
            Assert.AreSame(reconciliation, result.Reconciliation);
        }

        [TestMethod]
        public void NewInstance_When_Reconciliation_Is_Null_Should_Return_Object()
        {
            // Arrange
            var feedId = Guid.NewGuid();

            // Act
            var result = FeedReconciliation.NewInstance(null, feedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(feedId, result.FeedId);
            Assert.IsNull(result.Reconciliation);
        }
    }
}