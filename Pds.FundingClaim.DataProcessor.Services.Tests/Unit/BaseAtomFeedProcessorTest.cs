using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace Pds.FundingClaim.DataProcessor.Services.Tests.Unit
{
    public abstract class BaseAtomFeedProcessorTest
    {
        #region Assertion Helpers

        protected void AssertItemsReturnedFromFeed(IEnumerable<SyndicationItem> items, params int[] ids)
        {
            var itemsAsList = items.Where(o => o != null).ToList();

            Assert.AreEqual(ids.Length, itemsAsList.Count);
            for (var index = 0; index < ids.Length; index++)
            {
                var id = ids[index];
                Assert.AreEqual(CreateId(id), itemsAsList[index].Id);
            }
        }

        protected void AssertNewBookmark(Guid newBookmark, Guid expectedNewBookmark)
        {
            Assert.AreEqual(expectedNewBookmark, newBookmark);
        }

        #endregion


        #region SyndicationFeed Helpers

        protected GuidItemIdBasedAtomFeedProcessor GetAtomFeedProcessor(Action<string> afterEachPageLoad, bool page0Empty = false, bool page1Empty = false, bool page2Empty = false)
        {
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(page0Empty ? new int[0] : new[] { 1, 2 }, nextUrl: "http://page1/"));

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page1/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(page1Empty ? new int[0] : new[] { 3, 4 }, previousUrl: "http://page0/"));

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page2/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(page2Empty ? new int[0] : new[] { 5, 6 }, previousUrl: "http://page1", isArchive: false));

            return new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);
        }

        protected GuidItemIdBasedAtomFeedProcessor GetGrowingAtomFeedProcessor(Action<string> afterEachPageLoad)
        {
            var mockHttpService = new Mock<IHttpService>();
            var logger = new Mock<ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor>>();

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page0/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new[] { 1, 2 }, nextUrl: null));

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page1/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new[] { 3, 4 }, previousUrl: "http://page0/", isArchive: false));

            mockHttpService.Setup(service => service.GetAtomSyndicationFeed("http://page2/", afterEachPageLoad)).ReturnsAsync(GetFeedWithData(new[] { 5, 6 }, previousUrl: "http://page1", isArchive: false));

            return new GuidItemIdBasedAtomFeedProcessor(mockHttpService.Object, logger.Object);
        }

        protected SyndicationFeed GetFeedWithData(IEnumerable<int> indexes, string previousUrl = null, string nextUrl = null, bool isArchive = true)
        {
            var feed = CreateEmptyFeed(previousUrl, nextUrl, isArchive);
            feed.Items = indexes.Select(CreateItem).ToList();
            return feed;
        }

        protected abstract string CreateId(int id);

        private SyndicationItem CreateItem(int id)
        {
            return new SyndicationItem
            {
                Title = new TextSyndicationContent("Item " + id),
                Id = CreateId(id),
                Content = SyndicationContent.CreatePlaintextContent("This is the content for Item " + id)
            };
        }

        private SyndicationFeed CreateEmptyFeed(string previousUrl = null, string nextUrl = null, bool isArchive = true)
        {
            var feed = new SyndicationFeed();

            if (previousUrl != null)
            {
                feed.Links.Add(new SyndicationLink(new Uri(previousUrl), "prev-archive", "prev link", "text/html", 1000));
            }

            if (nextUrl != null)
            {
                feed.Links.Add(new SyndicationLink(new Uri(nextUrl), "next-archive", "next link", "text/html", 1000));
            }

            if (isArchive)
            {
                feed.ElementExtensions.Add("archive", "http://purl.org/syndication/history/1.0", string.Empty);
            }

            return feed;
        }

        #endregion
    }
}