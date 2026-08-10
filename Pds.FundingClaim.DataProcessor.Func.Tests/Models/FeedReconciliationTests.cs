using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.CorporateSchema.Reconciliations;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;

namespace Pds.FundingClaim.DataProcessor.Func.Tests.Models
{
    [TestClass]
    public class FeedReconciliationTests
    {
        [TestMethod]
        public void NewInstance_Should_Return_FeedReconciliation_With_Expected_Values()
        {
            // Arrange
            var fcReconciliation = new FCReconciliation();
            var feedId = Guid.NewGuid();

            // Act
            var result = FeedReconciliation.NewInstance(
                fcReconciliation,
                feedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(feedId, result.FeedId);
            Assert.AreSame(fcReconciliation, result.Reconciliation);
        }

        [TestMethod]
        public void NewInstance_When_Reconciliation_Is_Null_Should_Set_Reconciliation_To_Null()
        {
            // Arrange
            FCReconciliation fcReconciliation = null;
            var feedId = Guid.NewGuid();

            // Act
            var result = FeedReconciliation.NewInstance(
                fcReconciliation,
                feedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(feedId, result.FeedId);
            Assert.IsNull(result.Reconciliation);
        }
    }
}