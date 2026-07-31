namespace Pds.FundingClaim.DataProcessor.Services.Constants
{
    /// <summary>
    /// Contains all the constants used across service layer.
    /// </summary>
    public static class ServiceConstants
    {
        /// <summary>
        /// This represents the contigency time period that is to be subtracted to funding claim last retrieved so that we don't miss any funding claims in the period.
        /// </summary>
        public const int ContigencyPeriodInHours = -4;

        /// <summary>
        /// Page size to be passed to DC API so that we we get all results in one go.
        /// </summary>
        public const int PageSizeForDCApiCall = int.MaxValue;
    }
}