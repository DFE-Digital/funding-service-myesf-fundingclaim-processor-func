using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.DataProcessor.Services.Extensions;
using System;
using System.Xml.Linq;
using ServiceXElementExtensions = Pds.FundingClaim.DataProcessor.Services.Extensions.XElementExtensions;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Extensions
{
    [TestClass]
    public class XElementExtensionsTests
    {
        #region LowerCaseAllElementNames

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LowerCaseAllElementNames_NullPassed()
        {
            // Act
            ServiceXElementExtensions.LowerCaseAllElementNames(null);
        }

        [TestMethod, TestCategory("Unit")]
        public void LowerCaseAllElementNames()
        {
            // Arrange
            var xml = "<xMl xmlns:xs=\"www.Madeup.com\"><xs:parent><xs:CHILD aTTr1=\"some value\"/></xs:parent></xMl>";
            var expected = "<xml xmlns:xs=\"www.Madeup.com\"><xs:parent><xs:child aTTr1=\"some value\" /></xs:parent></xml>";
            var doc = XDocument.Parse(xml);

            // Act
            var actual = doc.Root.LowerCaseAllElementNames().ToString(SaveOptions.DisableFormatting);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        #endregion


        #region LowerCaseAllAttributeNames

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LowerCaseAllAttributeNames_NullPassed()
        {
            // Act
            ServiceXElementExtensions.LowerCaseAllAttributeNames(null);
        }

        [TestMethod, TestCategory("Unit")]
        public void LowerCaseAllAttributeNames()
        {
            // Arrange
            var xml = "<xMl xmlns:xs=\"www.Madeup.com\"><xs:parent><xs:CHILD aTTr1=\"some value\"/></xs:parent></xMl>";
            var expected = "<xMl xmlns:xs=\"www.Madeup.com\"><xs:parent><xs:CHILD attr1=\"some value\" /></xs:parent></xMl>";
            var doc = XDocument.Parse(xml);

            // Act
            var actual = doc.Root.LowerCaseAllAttributeNames().ToString(SaveOptions.DisableFormatting);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        #endregion
    }
}