namespace Pds.FundingClaim.DataProcessor.Services.Models
{
    /// <summary>
    /// Defines a message detailing an exception that occurred during the process of a the feed.
    /// </summary>
    public class FeedReadExceptionMessage
    {
        /// <summary>
        /// The type of exception that was thrown whilst reading the feed.
        /// </summary>
        public enum ExceptionType
        {
            /// <summary>
            /// Bookmark Not Matched exception.
            /// </summary>
            BookmarkNotMatched = 0,

            /// <summary>
            /// Empty Page On Feed exception.
            /// </summary>
            EmptyPageOnFeed = 1
        }

        /// <summary>
        /// Gets or sets the URL that was being used at the time.
        /// </summary>
        public ExceptionType Type { get; set; }

        /// <summary>
        /// Gets or sets the bookmark that was not matched.
        /// </summary>
        public Guid Bookmark { get; set; }

        /// <summary>
        /// Gets or sets the URL that was being used at the time.
        /// </summary>
        public string Url { get; set; }
    }
}
