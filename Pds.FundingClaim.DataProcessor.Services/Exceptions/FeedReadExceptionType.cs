namespace Pds.FundingClaim.DataProcessor.Services.Exceptions
{
    /// <summary>
    /// The type of exception that was thrown whilst reading the feed.
    /// </summary>
    public enum FeedReadExceptionType
    {
        /// <summary>
        /// The last read bookmarkid didn't match.
        /// </summary>
        BookmarkNotMatched = 0,

        /// <summary>
        /// Feed read returned empty page.
        /// </summary>
        EmptyPageOnFeed = 1
    }
}
