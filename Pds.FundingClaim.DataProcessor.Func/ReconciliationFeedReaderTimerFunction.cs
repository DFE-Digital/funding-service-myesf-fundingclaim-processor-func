using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;

namespace Pds.FundingClaim.DataProcessor.Func
{
    /// <summary>
    /// Reconciliation feed reader timer triggered Azure Function.
    /// </summary>
    public class ReconciliationFeedReaderTimerFunction
    {
        private readonly IReconciliationService _reconciliationService;
        private readonly ISystemProvider _systemProvider;
        private readonly ILoggerAdapter<ReconciliationFeedReaderTimerFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationFeedReaderTimerFunction"/> class.
        /// </summary>
        /// <param name="reconciliationService">The reconciliation service.</param>
        /// <param name="systemProvider">The system provider.</param>
        /// <param name="logger">The logger.</param>
        public ReconciliationFeedReaderTimerFunction(
            IReconciliationService reconciliationService,
            ISystemProvider systemProvider,
            ILoggerAdapter<ReconciliationFeedReaderTimerFunction> logger)
        {
            _reconciliationService = reconciliationService;
            _systemProvider = systemProvider;
            _logger = logger;
        }

        /// <summary>
        /// Entry point to the Azure Function.
        /// </summary>
        /// <param name="myTimer">The timer info.</param>
        /// <returns>Async task.</returns>
        [Function("ReconciliationFeedReaderTimerFunction")]
        public async Task Run(
            [TimerTrigger("%ReconciliationFeedReaderScheduleTriggerTime%")] TimerInfo myTimer)
        {
            await _reconciliationService.ReadAndProcessReconciliationFeed();

            _logger.LogInformation(
                $"ReconciliationFeedReader Timer trigger function executed at: {_systemProvider.Now()}.");
        }
    }
}