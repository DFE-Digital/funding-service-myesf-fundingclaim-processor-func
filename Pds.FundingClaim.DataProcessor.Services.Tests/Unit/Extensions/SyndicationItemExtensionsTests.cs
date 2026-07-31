using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.DataProcessor.Services.Extensions;
using System;
using System.ServiceModel.Syndication;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Extensions
{
    [TestClass]
    public class SyndicationItemExtensionsTests
    {
        [TestMethod, TestCategory("Unit")]
        public void IdAsGuid_IdWithUuid()
        {
            // Arrange
            var syndicationItem = new SyndicationItem { Id = "uuid:fdcfcfcd-764a-46a7-b8cf-5b87d15906fa" };
            var expected = new Guid("fdcfcfcd-764a-46a7-b8cf-5b87d15906fa");

            // Act
            var actual = syndicationItem.IdAsGuid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod, TestCategory("Unit")]
        public void IdAsGuid_IdWithoutUuid()
        {
            // Arrange
            var syndicationItem = new SyndicationItem { Id = "fdcfcfcd-764a-46a7-b8cf-5b87d15906fa" };
            var expected = new Guid("fdcfcfcd-764a-46a7-b8cf-5b87d15906fa");

            // Act
            var actual = syndicationItem.IdAsGuid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}