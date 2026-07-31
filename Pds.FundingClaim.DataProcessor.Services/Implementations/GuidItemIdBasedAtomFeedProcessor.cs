using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Exceptions;
using Pds.FundingClaim.DataProcessor.Services.Extensions;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IFeedProcessor" />
    public class GuidItemIdBasedAtomFeedProcessor : IFeedProcessor
    {
        private readonly IHttpService _httpService;
        private readonly ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidItemIdBasedAtomFeedProcessor"/> class.
        /// Default Constructor.
        /// </summary>
        /// <param name="httpService">The http service.</param>
        /// <param name="logger">The logger.</param>
        public GuidItemIdBasedAtomFeedProcessor(
            IHttpService httpService, ILoggerAdapter<GuidItemIdBasedAtomFeedProcessor> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IList<T>> ReadAndProcessAfterMatch<T>(string initialUrl, Guid lastReadItemId, Func<SyndicationItem, T> processItem, Action<string> afterEachPageLoad)
        {
            Func<SyndicationItem, bool> isPreviouslyReadItem = item =>
            {
                var eventId = item.IdAsGuid();

                // We have found the original bookmark, therefore we don't want to process this contract or any before it
                return eventId == lastReadItemId;
            };

            Action startOfFeedReached = () =>
            {
                // We're now on the first page of the feed (not the first read page) as the uri is null
                if (lastReadItemId != Guid.Empty)
                {
                    // This means that we have expected to have matched an event somewhere but we haven't
                    throw new GuidBookmarkNotMatchedException(lastReadItemId, initialUrl);
                }
            };

            var result = await ReadAndProcessAfterMatch(initialUrl, isPreviouslyReadItem, processItem, afterEachPageLoad, startOfFeedReached);

            return result;
        }

        /// <summary>
        /// Reads the feed from the place last read, which is defined by <paramref name="isPreviouslyReadItem"/> and returns the content processed by <paramref name="processItem"/>.
        /// </summary>
        /// <typeparam name="T">The type of item that is expected to be in the content of each item within the feed once its been processed.</typeparam>
        /// <param name="subscriptionDocumentUrl">The initial page to read for the feed.</param>
        /// <param name="isPreviouslyReadItem">Determines if the item passed has already been read. If <c>true</c> then only items that occur after this in the feed will be processed and returned.</param>
        /// <param name="processItem">Defines a function which takes each <see cref="SyndicationItem"/> and returns the desired item.</param>
        /// <param name="afterEachPageLoad">Action to take after each page load.</param>
        /// <param name="startOfFeedReached">Triggered when all items on the feed have been read and no previous item has been found.</param>
        /// <returns>An enumerable list of processed items from a feed starting with the first item after a previous item match, if such a match exists.</returns>
        private async Task<IList<T>> ReadAndProcessAfterMatch<T>(string subscriptionDocumentUrl, Func<SyndicationItem, bool> isPreviouslyReadItem, Func<SyndicationItem, T> processItem, Action<string> afterEachPageLoad, Action startOfFeedReached = null)
        {
            // We would really like to go back and match the item to start reading from and then work forwards.
            // However the spec says that the penultimate (latest) archive shouldn't have a next-archive link.
            // Because of this, we have to store the pages as we read them and take the memory hit until we
            // find where we are starting to read from.
            var currentUri = subscriptionDocumentUrl;
            var items = new List<T>();

            while (currentUri != null)
            {
                var currentFeed = await _httpService.GetAtomSyndicationFeed(currentUri, afterEachPageLoad);
                CheckForEmptyPage(currentFeed, currentUri);

                foreach (var item in currentFeed.Items.Reverse())
                {
                    var url = currentUri;
                    if (isPreviouslyReadItem(item))
                    {
                        _logger.LogDebug($"Matched the item with id {item.Id} on {url}. Stop reading any more items.");
                        return items;
                    }

                    _logger.LogDebug($"Unmatched item with event id {item.Id} on {url}.");
                    items.Insert(0, processItem(item));
                }

                var previousArchiveUri = PreviousArchiveUri(currentFeed);

                if (previousArchiveUri == null)
                {
                    startOfFeedReached?.Invoke();
                }

                currentUri = previousArchiveUri?.ToString();
            }

            return items;
        }

        #region Helpers

        /// <summary>
        /// The only allowed empty page is when there are no items at all. In this scenario, the subscription page would have no items and no previous link
        /// all other cases are an error.
        /// </summary>
        /// <param name="document">The current page being looked at.</param>
        /// <param name="currentUri">The uri of the current page being looked at.</param>
        /// <exception cref="EmptyPageOnFeedException">Thrown if the page is empty and is not the first and subscription page.</exception>
        private void CheckForEmptyPage(SyndicationFeed document, string currentUri)
        {
            var previousArchiveUri = PreviousArchiveUri(document);
            var isFirstPage = previousArchiveUri == null;

            if (!document.Items.Any() && !(isFirstPage && !IsArchiveDocument(document)))
            {
                throw new EmptyPageOnFeedException(currentUri);
            }
        }

        /// <summary>
        /// Returns a flag indicating if the document is an archive document.
        /// </summary>
        /// <param name="document">The document to check whether or not it is an archive document.</param>
        /// <returns><c>true</c> if an archive element exists; otherwise, <c>false</c>.</returns>
        private bool IsArchiveDocument(SyndicationFeed document) => document.ElementExtensions.Any(o => o.OuterName.ToLower() == "archive");


        /// <summary>
        /// Returns the uri of the previous archive document if the link exists.
        /// </summary>
        /// <param name="document">The document to get the link from.</param>
        /// <returns>The uri, or <c>null</c> if no link exists.</returns>
        private Uri PreviousArchiveUri(SyndicationFeed document) => document.Links.SingleOrDefault(l => l.RelationshipType == "prev-archive")?.Uri;

        #endregion
    }
}