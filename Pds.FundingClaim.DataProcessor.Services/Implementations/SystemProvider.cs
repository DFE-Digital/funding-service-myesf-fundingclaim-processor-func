using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="ISystemProvider"/>
    public class SystemProvider : ISystemProvider
    {
        /// <inheritdoc/>
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}