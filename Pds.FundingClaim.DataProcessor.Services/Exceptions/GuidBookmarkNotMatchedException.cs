namespace Pds.FundingClaim.DataProcessor.Services.Exceptions
{
    /// <summary>
    /// Thrown when a feed was read but an existing bookmark was not matched.
    /// </summary>
    public class GuidBookmarkNotMatchedException : BaseFeedReadException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidBookmarkNotMatchedException"/> class.
        /// </summary>
        /// <param name="bookmark">The bookmark that was not matched.</param>
        /// <param name="url">The URL that the empty page was found at.</param>
        public GuidBookmarkNotMatchedException(Guid bookmark, string url)
            : base($"The bookmark [{bookmark}] was not matched and we have reached the beginning of the feed.", url)
        {
        }
    }
}
