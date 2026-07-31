using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;

namespace Pds.FundingClaim.DataProcessor.Func
{
    /// <summary>
    /// Get Funding Claim timer triggered Azure Function.
    /// </summary>
    public class GetFundingClaimTimerFunction
    {
        private readonly IFundingClaimService _fundingClaimService;
        private readonly ISystemProvider _systemProvider;
        private readonly ILoggerAdapter<GetFundingClaimTimerFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFundingClaimTimerFunction"/> class.
        /// </summary>
        /// <param name="fundingClaimService">The funding claim service.</param>
        /// <param name="systemProvider">The system provider.</param>
        /// <param name="logger">The logger.</param>
        public GetFundingClaimTimerFunction(
            IFundingClaimService fundingClaimService,
            ISystemProvider systemProvider,
            ILoggerAdapter<GetFundingClaimTimerFunction> logger)
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
        [Function("GetFundingClaimTimerFunction")]
        public async Task Run(
            [TimerTrigger("%GetFundingClaimScheduleTriggerTime%")] TimerInfo myTimer)
        {
            _logger.LogInformation($"GetFundingClaim timer trigger function started at: {_systemProvider.Now()}.");

            var useNewDCAPI = await _fundingClaimService.ShouldUseNewDCAPI();

            if (useNewDCAPI)
            {
                await _fundingClaimService.GetFundingClaims();
            }
            else
            {
                _logger.LogInformation($"Use new DC restful API is turned OFF.");
            }

            _logger.LogInformation($"GetFundingClaim timer trigger function finished at: {_systemProvider.Now()}.");
        }
    }
}