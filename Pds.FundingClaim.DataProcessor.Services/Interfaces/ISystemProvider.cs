using System;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// The datetime provider to help with datetime operations.
    /// </summary>
    public interface ISystemProvider
    {
        /// <summary>
        /// Gets the current datetime.
        /// </summary>
        /// <returns>Returns the current datetime.</returns>
        DateTime Now();
    }
}