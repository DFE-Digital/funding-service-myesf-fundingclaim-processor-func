using AutoMapper;
using Pds.FundingClaim.CorporateSchema.Reconciliations;

namespace Pds.FundingClaim.DataProcessor.Services.Models
{
    /// <summary>
    /// Represents the syndicate feed item read from FCS atom feed.
    /// </summary>
    public class FeedReconciliation
    {
        private static readonly IMapper Mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<FCReconciliation, FeedReconciliation>();
        }).CreateMapper();

        /// <summary>
        /// Gets or sets the syndicate feed identifier.
        /// </summary>
        public Guid FeedId { get; set; }

        /// <summary>
        /// Gets or sets the corporate reconciliation object.
        /// </summary>
        public FCReconciliation Reconciliation { get; set; }

        #region Main Api

        /// <summary>
        /// Build the feed funding claim reconciliation instance from the corporate reconciliationToCopy type passed by adding feed id to it.
        /// </summary>
        /// <param name="fcReconciliation">The reconciliation for which the feed reconciliationToCopy is built.</param>
        /// <param name="feedId">The FCS unique identifier associated with the corporate reconciliation.</param>
        /// <returns>FeedReconciliation instance.</returns>
        public static FeedReconciliation NewInstance(FCReconciliation fcReconciliation, Guid feedId)
        {
            return NewInstance(fcReconciliation, fcReconciliation, feedId);
        }

        private static FeedReconciliation NewInstance(FCReconciliation masterReconciliation, FCReconciliation reconciliationToCopy, Guid feedId)
        {
            var feedReconciliation = Mapper.Map<FCReconciliation, FeedReconciliation>(reconciliationToCopy);
            feedReconciliation.FeedId = feedId;
            feedReconciliation.Reconciliation = masterReconciliation;

            return feedReconciliation;
        }

        #endregion
    }
}