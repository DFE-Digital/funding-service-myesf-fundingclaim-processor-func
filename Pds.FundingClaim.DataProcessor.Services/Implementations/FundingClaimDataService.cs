using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.FundingClaims;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Constants;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using System.Collections.Specialized;
using CorporateFundingClaim = Pds.FundingClaim.CorporateSchema.FundingClaims.FundingClaim;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IFundingClaimDataService"/>
    public class FundingClaimDataService : IFundingClaimDataService
    {
        private readonly IHttpService _httpService;
        private readonly DataCollectionApiEndpointSettings _apiSettings;
        private readonly ILoggerAdapter<FundingClaimDataService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingClaimDataService"/> class.
        /// </summary>
        /// <param name="httpService">The http service.</param>
        /// <param name="apiSettings">The api settings.</param>
        /// <param name="logger">The logger.</param>
        public FundingClaimDataService(
            IHttpService httpService,
            IOptions<DataCollectionApiEndpointSettings> apiSettings,
            ILoggerAdapter<FundingClaimDataService> logger)
        {
            _httpService = httpService;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FundingClaimDetails>> GetFundingClaimWindowDetails()
        {
            var result = await _httpService.GetFromDCWithMSILAuthenticationAsync($"{_apiSettings.BaseUri}{_apiSettings.GetFundingClaimCollectionsDetailsEndpoint}");

            _logger.LogInformation(
                $"FundingClaimDataService successfully executed GetFundingClaimWindowDetails for uri {_apiSettings.BaseUri}{_apiSettings.GetFundingClaimCollectionsDetailsEndpoint}.");

            var fundingClaimDetails = JsonConvert.DeserializeObject<IList<FundingClaimDetails>>(result);

            _logger.LogInformation($"FundingClaimDataService GetFundingClaimWindowDetails retrieved {fundingClaimDetails.Count} records");

            return fundingClaimDetails;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CorporateFundingClaim>> GetFundingClaim(DateTime sinceDateTime, bool requireSignature)
        {
            var parameters = new NameValueCollection
            {
                { "sinceDateTime", sinceDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "requireSignature", requireSignature.ToString() },
                { "pageSize", ServiceConstants.PageSizeForDCApiCall.ToString() }
            };

            var result =
                await _httpService.GetFromDCWithMSILAuthenticationAsync(
                    $"{_apiSettings.BaseUri}{_apiSettings.GetFundingClaimsEndpoint}", parameters);

            var log = "FundingClaimDataService successfully executed GetFundingClaim to uri "
                       + $"{_apiSettings.BaseUri}{_apiSettings.GetFundingClaimsEndpoint}." + Environment.NewLine
                       + "It sent the request with the following parameters:" + Environment.NewLine
                       + $"   sinceDateTime: {parameters["sinceDateTime"]}" + Environment.NewLine
                       + $"   requireSignature: {parameters["requireSignature"]}" + Environment.NewLine
                       + $"   pageSize: {parameters["pageSize"]}";

            _logger.LogInformation(log);

            var fundingClaims = JsonConvert.DeserializeObject<IList<CorporateFundingClaim>>(result);
            _logger.LogInformation(fundingClaims == null
                ? $"FundingClaimDataService has not returned any funding claims data"
                : $"FundingClaimDataService has returned {fundingClaims.Count} funding claims.");

            return fundingClaims;
        }
    }
}