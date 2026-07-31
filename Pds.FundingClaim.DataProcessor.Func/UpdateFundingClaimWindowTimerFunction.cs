using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;

namespace Pds.FundingClaim.DataProcessor.Func
{
    /// <summary>
    /// Update Funding Claim Window timer triggered Azure Function.
    /// </summary>
    public class UpdateFundingClaimWindowTimerFunction
    {
        private readonly IFundingClaimService _fundingClaimService;
        private readonly ISystemProvider _systemProvider;
        private readonly ILoggerAdapter<UpdateFundingClaimWindowTimerFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateFundingClaimWindowTimerFunction"/> class.
        /// </summary>
        /// <param name="fundingClaimService">The funding claim service.</param>
        /// <param name="systemProvider">The system provider.</param>
        /// <param name="logger">The logger.</param>
        public UpdateFundingClaimWindowTimerFunction(
            IFundingClaimService fundingClaimService,
            ISystemProvider systemProvider,
            ILoggerAdapter<UpdateFundingClaimWindowTimerFunction> logger)
        {
            _fundingClaimService = fundingClaimService;
            _systemProvider = systemProvider;
            _logger = logger;
        }

        /// <summary>
        /// Entry point to the Azure Function.
        /// </summary>
        /// <param name="myTimer">The timer info.</param>
        /// <returns>Async task.</returns>
        [Function("UpdateFundingClaimWindowTimerFunction")]
        public async Task Run(
            [TimerTrigger("%UpdateFundingClaimWindowScheduleTriggerTime%")] TimerInfo myTimer)
        {
            _logger.LogInformation($"UpdateFundingClaimWindow Timer trigger function started at: {_systemProvider.Now()}.");

            var useNewDCAPI = await _fundingClaimService.ShouldUseNewDCAPI();

            if (useNewDCAPI)
            {
                await _fundingClaimService.UpdateFundingClaimWindows();
            }
            else
            {
                _logger.LogInformation($"Use new DC restful API is turned OFF.");
            }

            _logger.LogInformation($"UpdateFundingClaimWindow Timer trigger function finished at: {_systemProvider.Now()}.");
        }
    }
}