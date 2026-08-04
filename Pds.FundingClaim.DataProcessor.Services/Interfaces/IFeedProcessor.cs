using System.ServiceModel.Syndication;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Atom feed processed where the item id is a <see cref="Guid"/> and is used as the way to determine the last item read from the feed.
    /// </summary>
    public interface IFeedProcessor
    {
        /// <summary>
        /// Reads the feed from the place last read and returns the content processed by <paramref name="processItem"/>.
        /// </summary>
        /// <typeparam name="T">The type of item that is expected to be in the content of each item within the feed once its been processed.</typeparam>
        /// <param name="initialUrl">The initial page to read for the feed.</param>
        /// <param name="lastReadItemId">The last Item Id read by a previous read of the feed.</param>
        /// <param name="processItem">Defines a function which takes each <see cref="SyndicationItem"/> and returns the desired item.</param>
        /// <param name="afterEachPageLoad">Action to take after each page load.</param>
        /// <returns>An enumerable list of processed items from a feed starting with the first item after a previous item match, if such a match exists.</returns>
        Task<IList<T>> ReadAndProcessAfterMatch<T>(string initialUrl, Guid lastReadItemId, Func<SyndicationItem, T> processItem, Action<string> afterEachPageLoad);
    }
}