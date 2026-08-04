namespace Pds.FundingClaim.DataProcessor.Services.Extensions
{
    /// <summary>
    /// Extension class for string types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts string to UTC datetime.
        /// </summary>
        /// <returns>Returns the utc date time equivalent.</returns>
        /// <param name="dateTimeString">The date time string to be converted.</param>
        public static DateTime ToUtcDateTime(this string dateTimeString)
        {
            return DateTimeOffset.Parse(dateTimeString).UtcDateTime;
        }

        /// <summary>
        /// Converts string to guid.
        /// </summary>
        /// <returns>Returns the utc date time equivalent.</returns>
        /// <param name="guid">The guid string to be converted.</param>
        public static Guid ToGuid(this string guid)
        {
            return Guid.Parse(guid);
        }

        /// <summary>
        /// Converts string to timespan.
        /// </summary>
        /// <returns>Returns the timespan equivalent.</returns>
        /// <param name="timespan">The timespan string to be converted.</param>
        public static TimeSpan ToTimeSpan(this string timespan)
        {
            return TimeSpan.Parse(timespan);
        }

        /// <summary>
        /// Checks if the string vlaue is equal to true.
        /// </summary>
        /// <returns>Returns the check result as true/false.</returns>
        /// <param name="term">The string to be checked.</param>
        public static bool IsTrue(this string term)
        {
            return bool.TrueString.Equals(term, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}