namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Represents the deliverable code for the contract allocations.
    /// </summary>
    public class DeliverableCode
    {
        /// <summary>
        /// Gets or sets the Deliverable Code Value.
        /// </summary>
        public int DeliverableCodeValue { get; set; }

        /// <summary>
        /// Gets or sets the Actual Volume.
        /// </summary>
        public int ActualVolume { get; set; }

        /// <summary>
        /// Gets or sets the Actual Value.
        /// </summary>
        public decimal ActualValue { get; set; }

        /// <summary>
        /// Gets or sets the Forecast Value.
        /// </summary>
        public decimal ForecastValue { get; set; }

        /// <summary>
        /// Gets or sets the Adjustment Value.
        /// </summary>
        public decimal AdjustmentValue { get; set; }

        /// <summary>
        /// Gets or sets the Total Delivery.
        /// </summary>
        public decimal TotalDelivery { get; set; }
    }
}