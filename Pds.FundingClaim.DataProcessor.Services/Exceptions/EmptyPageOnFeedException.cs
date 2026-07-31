namespace Pds.FundingClaim.DataProcessor.Services.Exceptions
{
    /// <summary>
    /// Thrown when a feed was read but an existing bookmark was not matched.
    /// </summary>
    public class EmptyPageOnFeedException : BaseFeedReadException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPageOnFeedException"/> class.
        /// </summary>
        /// <param name="url">The URL of the empty page.</param>
        public EmptyPageOnFeedException(string url)
            : base($"An empty page was found in the ATOM feed at page [{url}].", url)
        {
        }
    }
}
