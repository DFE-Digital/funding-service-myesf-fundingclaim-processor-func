using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;

namespace Pds.FundingClaim.DataProcessor.Func
{
    /// <summary>
    /// Update Funding Claim Window timer triggered Azure Function.
    /// </summary>
    public class AutowithdrawFundingClaimTimerFunction
    {
        private readonly IFundingClaimService _fundingClaimService;
        private readonly ISystemProvider _systemProvider;
        private readonly ILoggerAdapter<AutowithdrawFundingClaimTimerFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutowithdrawFundingClaimTimerFunction"/> class.
        /// </summary>
        /// <param name="fundingClaimService">The funding claim service.</param>
        /// <param name="systemProvider">The system provider.</param>
        /// <param name="logger">The logger.</param>
        public AutowithdrawFundingClaimTimerFunction(
            IFundingClaimService fundingClaimService,
            ISystemProvider systemProvider,
            ILoggerAdapter<AutowithdrawFundingClaimTimerFunction> logger)
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
        [Function("AutowithdrawFundingClaimTimerFunction")]
        public async Task Run(
            [TimerTrigger("%AutowithdrawFundingClaimScheduleTriggerTime%")] TimerInfo myTimer)
        {
            _logger.LogInformation($"AutowithdrawFundingClaim timer trigger function started at: {_systemProvider.Now()}.");

            var useNewDCAPI = await _fundingClaimService.ShouldUseNewDCAPI();

            if (useNewDCAPI)
            {
                await _fundingClaimService.AutowithdrawFundingClaims();
            }
            else
            {
                _logger.LogInformation($"Use new DC restful API is turned OFF.");
            }

            _logger.LogInformation($"AutowithdrawFundingClaim timer trigger function finished at: {_systemProvider.Now()}.");
        }
    }
}