using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Exceptions;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
      [TestClass]
      public class GuidItemIdBasedAtomFeedProcessorTests : BaseAtomFeedProcessorTest
    {
        #region ReadAndProcessAfterMatch


        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_AllItemsRead()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Parse("00000000-0000-0000-0000-000000000006"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_NoItemsRead()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Empty,
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual, 1, 2, 3, 4, 5, 6);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_MatchOnPage2()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Parse("00000000-0000-0000-0000-000000000005"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual, 6);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_MatchOnPage0AtEnd()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual, 3, 4, 5, 6);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_2Pages_PageAddedWhenReading_MatchOnPage0AtEnd()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetGrowingAtomFeedProcessor(afterEachPageLoad);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page1/",
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual, 3, 4);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_1Page_AllItemsRead()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();
            var uri = "http://page0/";
            Action<string> afterEachPageLoad = lastReadUrl => { };

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new[] { 1, 2 }, isArchive: false));

            var atomFeedProcessor = new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                uri,
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_1Page_NoItemsRead()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();
            Action<string> afterEachPageLoad = lastReadUrl => { };

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new[] { 1, 2 }, isArchive: false));

            var atomFeedProcessor = new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page0/",
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual, 1, 2);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_1Page_SomeItemsRead()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();
            Action<string> afterEachPageLoad = lastReadUrl => { };

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new[] { 1, 2 }, isArchive: false));

            var atomFeedProcessor = new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page0/",
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual, 2);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_1Page_NoItems()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();
            Action<string> afterEachPageLoad = lastReadUrl => { };

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new int[] { }, isArchive: false));

            var atomFeedProcessor = new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page0/",
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual);
        }

        [ExpectedException(typeof(EmptyPageOnFeedException))]
        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_Page0Empty()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad, page0Empty: true);

            // Act
            await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                item => item,
                afterEachPageLoad);
        }

        [ExpectedException(typeof(EmptyPageOnFeedException))]
        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_Page1Empty()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad, page1Empty: true);

            // Act
            await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                item => item,
                afterEachPageLoad);
        }

        [ExpectedException(typeof(EmptyPageOnFeedException))]
        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_Page2Empty()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad, page2Empty: true);

            // Act
            await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                item => item,
                afterEachPageLoad);
        }

        [ExpectedException(typeof(GuidBookmarkNotMatchedException))]
        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_3Pages_BookmarkNotMatched()
        {
            // Arrange
            Action<string> afterEachPageLoad = lastReadUrl => { };
            var atomFeedProcessor = GetAtomFeedProcessor(afterEachPageLoad);

            // Act
            await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page2/",
                Guid.NewGuid(),
                item => item,
                afterEachPageLoad);
        }


        [ExpectedException(typeof(GuidBookmarkNotMatchedException))]
        [TestMethod, TestCategory("Unit")]
        public async Task ReadAndProcessAfterMatch_1Page_NoItems_BadBookmark()
        {
            // Arrange
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();
            Action<string> afterEachPageLoad = lastReadUrl => { };

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new int[] { }, isArchive: false));

            var atomFeedProcessor = new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);

            // Act
            var actual = await atomFeedProcessor.ReadAndProcessAfterMatch(
                "http://page0/",
                Guid.NewGuid(),
                item => item,
                afterEachPageLoad);

            // Assert
            AssertItemsReturnedFromFeed(actual);
        }

        #endregion

        #region Overrides


        protected override string CreateId(int id)
        {
            return $"uuid:00000000-0000-0000-0000-00000000000{id}";
        }


        #endregion
    }
}