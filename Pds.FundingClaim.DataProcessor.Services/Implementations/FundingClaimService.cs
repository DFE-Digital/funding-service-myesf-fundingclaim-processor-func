using Pds.Core.Logging;
using Pds.FundingClaim.DataProcessor.Services.Constants;
using Pds.FundingClaim.DataProcessor.Services.Extensions;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IFundingClaimService"/>
    public class FundingClaimService : IFundingClaimService
    {
        private readonly IFundingClaimApiService _fundingClaimApiService;
        private readonly IFundingClaimDataService _fundingClaimDataService;
        private readonly ILoggerAdapter<FundingClaimService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingClaimService"/> class.
        /// </summary>
        /// <param name="fundingClaimApiService">The Funding Claim Api Service.</param>
        /// <param name="fundingClaimDataService">The Funding Claim Data Service.</param>
        /// <param name="logger">The logger.</param>
        public FundingClaimService(
            IFundingClaimApiService fundingClaimApiService, IFundingClaimDataService fundingClaimDataService, ILoggerAdapter<FundingClaimService> logger)
        {
            _fundingClaimApiService = fundingClaimApiService;
            _fundingClaimDataService = fundingClaimDataService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> ShouldUseNewDCAPI()
        {
            var useNewAPI = await _fundingClaimApiService.GetUseJsonFormatOfFundingClaimsSetting();

            return useNewAPI.IsTrue();
        }

        /// <inheritdoc/>
        public async Task UpdateFundingClaimWindows()
        {
            if (await IsFundingClaimsRetrievalAllowed())
            {
                var fundingClaimDetails = await _fundingClaimDataService.GetFundingClaimWindowDetails();

                await _fundingClaimApiService.UpdateFundingClaimWindows(fundingClaimDetails.ToList());

                _logger.LogInformation("FundingClaimService successfully executed UpdateFundingClaimWindows.");
            }
            else
            {
                _logger.LogInformation("Funding claim polling is turned OFF in settings table.");
            }
        }

        /// <inheritdoc/>
        public async Task GetFundingClaims()
        {
            if (await IsFundingClaimsRetrievalAllowed())
            {
                var fundingClaimCurrentWindow = await _fundingClaimApiService.GetFundingClaimCurrentWindow();

                if (fundingClaimCurrentWindow != null)
                {
                    var fundingClaimLastRetrievedAsString = await _fundingClaimApiService.GetFundingClaimLastRetrievedSetting();
                    var fundingClaimLastRetrievedDateTime = fundingClaimLastRetrievedAsString.ToUtcDateTime();

                    var sinceDateTime = fundingClaimLastRetrievedDateTime < fundingClaimCurrentWindow.SubmissionOpenDate ? fundingClaimCurrentWindow.SubmissionOpenDate : fundingClaimLastRetrievedDateTime.AddHours(ServiceConstants.ContigencyPeriodInHours);

                    var result = await _fundingClaimDataService.GetFundingClaim(sinceDateTime, fundingClaimCurrentWindow.RequiresSignature);

                    if (result != null)
                    {
                        await _fundingClaimApiService.UpdateFundingClaims(
                            new CreateFundingClaimsApiRequest
                            {
                                FundingClaims = result.ToList(),
                                FundingClaimWindowId = fundingClaimCurrentWindow.Id
                            });
                    }

                    CreateLogForGetFundingClaims(
                        fundingClaimCurrentWindow.RequiresSignature, getFundingClaimResultNotNull: result != null, sinceDateTime);
                }
            }
            else
            {
                _logger.LogInformation("Funding claim polling is turned OFF in settings table.");
            }
        }

        /// <inheritdoc/>
        public async Task AutowithdrawFundingClaims()
        {
            await _fundingClaimApiService.AutowithdrawFundingClaims();

            _logger.LogInformation("FundingClaimService successfully executed AutowithdrawFundingClaims.");
        }

        /// <summary>
        /// Checks if the funding claim polling setting is set to true.
        /// </summary>
        /// <returns>Returns whether funding claim polling setting is true or not.</returns>
        private async Task<bool> IsFundingClaimsRetrievalAllowed()
        {
            var fundingClaimPollingSetting = await _fundingClaimApiService.GetFundingClaimPollingSetting();

            return bool.TrueString.Equals(fundingClaimPollingSetting, System.StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Creates log for the GetFundingClaims method.
        /// </summary>
        /// <param name="fundingClaimWindowRequiresSignature">Boolean that captures whether the current window requires signature or not.</param>
        /// <param name="getFundingClaimResultNotNull">Boolean that captures whether GetFundingClaim's result is not null.</param>
        /// <param name="sinceDateTime">Date since the funding claims were retrieved.</param>
        private void CreateLogForGetFundingClaims(
            bool fundingClaimWindowRequiresSignature,
            bool getFundingClaimResultNotNull,
            DateTime sinceDateTime)
        {
            var requiresSignatureSubstring =
                fundingClaimWindowRequiresSignature ? string.Empty : " do not";

            var fundingClaimsRetrievedSubstring = getFundingClaimResultNotNull ? "Funding" : "No new funding";

            _logger.LogInformation(
                "FundingClaimService successfully executed GetFundingClaims for claims created since "
                + $"{sinceDateTime} that{requiresSignatureSubstring} require signature." + Environment.NewLine
                + $"{fundingClaimsRetrievedSubstring} claims were retrieved.");
        }
    }
}