using System;

namespace Pds.FundingClaim.DataProcessor.Services.Exceptions
{
    /// <summary>
    /// Base exception type for feed read custom exceptions.
    /// </summary>
    public abstract class BaseFeedReadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFeedReadException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="url">The URL of the empty page.</param>
        protected BaseFeedReadException(string message, string url)
            : base(message)
        {
            Url = url;
        }

        /// <summary>
        /// Gets or sets the URL that the empty page was found at.
        /// </summary>
        public string Url { get; protected set; }
    }
}
