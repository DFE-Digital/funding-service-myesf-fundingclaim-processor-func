using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.FundingClaims;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using Pds.FundingClaim.DataProcessor.Services.Models;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IFundingClaimApiService"/>
    public class FundingClaimApiService : IFundingClaimApiService
    {
        private readonly ILoggerAdapter<FundingClaimApiService> _logger;
        private readonly IHttpService _httpService;
        private readonly InternalApiSettings _apiSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingClaimApiService"/> class.
        /// </summary>
        /// <param name="httpService">The http service.</param>
        /// <param name="apiSettings">The api settings.</param>
        /// <param name="logger">The logger.</param>
        public FundingClaimApiService(
            IHttpService httpService, IOptions<InternalApiSettings> apiSettings, ILoggerAdapter<FundingClaimApiService> logger)
        {
            _httpService = httpService;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string> GetFundingClaimLastRetrievedSetting()
        {
            var uri = $"{_apiSettings.FundingClaimUri}GetFundingClaimLastRetrievedSetting";

            var result = await _httpService.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed GetFundingClaimLastRetrievedSetting for uri {uri}." + Environment.NewLine
                + $"Result is {result}.");

            return result;
        }

        /// <inheritdoc/>
        public async Task<string> GetFundingClaimPollingSetting()
        {
            var uri = $"{_apiSettings.FundingClaimUri}GetFundingClaimPollingSetting";

            var result = await _httpService.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed GetFundingClaimPollingSetting for uri {uri}." + Environment.NewLine
                + $"Result is {result}.");

            return result;
        }

        /// <inheritdoc/>
        public async Task<string> GetUseJsonFormatOfFundingClaimsSetting()
        {
            var uri = $"{_apiSettings.FundingClaimUri}GetUseJsonFormatOfFundingClaimsSetting";

            var result = await _httpService.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed GetUseJsonFormatOfFundingClaimsSetting for uri {uri}." + Environment.NewLine
                + $"Result is {result}.");

            return result;
        }

        /// <inheritdoc/>
        public async Task UpdateFundingClaimWindows(List<FundingClaimDetails> fundingClaimDetails)
        {
            var uri = $"{_apiSettings.FundingClaimUri}UpdateFundingClaimWindow";
            await _httpService.PostWithMSILAuthenticationAsync(uri, fundingClaimDetails);

            _logger.LogInformation(
                $"FundingClaimApiService successfully processed {fundingClaimDetails.Count} Funding claim windows. uri {uri}.");
        }

        /// <inheritdoc/>
        public async Task<FundingClaimWindow> GetFundingClaimCurrentWindow()
        {
            var uri = $"{_apiSettings.FundingClaimUri}GetFundingClaimCurrentWindow";

            var result = await _httpService.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri) ?? string.Empty;
            var window = JsonConvert.DeserializeObject<FundingClaimWindow>(result);

            var logSubstring = result == string.Empty ? "No window was found." : $"Retrieved FundingClaimWindow Id {window.Id}.";

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed GetFundingClaimCurrentWindow for uri {uri}." + Environment.NewLine
                + logSubstring);

            return window;
        }

        /// <inheritdoc/>
        public async Task UpdateFundingClaims(CreateFundingClaimsApiRequest fundingClaimsRequest)
        {
            var uri = $"{_apiSettings.FundingClaimUri}CreateFundingClaims";
            await _httpService.PostWithMSILAuthenticationAsync(uri, fundingClaimsRequest);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed UpdateFundingClaims for uri {uri}." + Environment.NewLine
                + $"The CreateFundingClaimsApiRequest was posted for FundingClaimWindow: {fundingClaimsRequest.FundingClaimWindowId}");
        }

        /// <inheritdoc/>
        public async Task AutowithdrawFundingClaims()
        {
            var uri = $"{_apiSettings.FundingClaimUri}AutoWithdrawFundingClaims";
            await _httpService.PutWithMSILAuthenticationAsync(uri);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed AutowithdrawFundingClaims for uri {uri}");
        }

        /// <inheritdoc/>
        public async Task<string> GetReconciliationFeedBookmarkIdSetting()
        {
            var uri = $"{_apiSettings.ReconciliationUri}GetReconciliationFeedBookmarkIdSetting";

            var result = await _httpService.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed GetReconciliationFeedBookmarkIdSetting for uri {uri}." + Environment.NewLine
                + $"Result is {result}.");

            return result;
        }

        /// <inheritdoc/>
        public async Task UpdateReconciliationFeedBookmarkId(Guid bookmarkId)
        {
            var uri = $"{_apiSettings.ReconciliationUri}UpdateReconciliationFeedBookmarkId";
            await _httpService.PostWithMSILAuthenticationAsync(uri, bookmarkId);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed UpdateReconciliationFeedBookmarkId for uri {uri}.");
        }

        /// <inheritdoc/>
        public async Task CreateReconciliation(FeedReconciliation reconciliation)
        {
            var uri = $"{_apiSettings.ReconciliationUri}CreateReconciliation";
            await _httpService.PostWithMSILAuthenticationAsync(uri, reconciliation);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed CreateReconciliation for uri {uri}.");
        }

        /// <inheritdoc/>
        public async Task AuditReconciliationFeedReadException(string message)
        {
            var uri = $"{_apiSettings.ReconciliationUri}AuditReconciliationFeedReadException";
            await _httpService.PostWithMSILAuthenticationAsync(uri, message);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed AuditReconciliationFeedReadException for uri {uri} and message {message}.");
        }

        /// <inheritdoc/>
        public async Task SendFeedReadExceptionEmail(FeedReadExceptionMessage message)
        {
            var uri = $"{_apiSettings.ReconciliationUri}SendFeedReadExceptionEmail";
            await _httpService.PostWithMSILAuthenticationAsync(uri, message);

            _logger.LogInformation(
                $"Creating azure service bus message to send email for feed exception. Bookmark[{message.Bookmark}], Url[{message.Url}]");
        }

        /// <inheritdoc/>
        public async Task<string> GetFeedReadWarningThresholdSetting()
        {
            var uri = $"{_apiSettings.ReconciliationUri}GetFeedReadWarningThresholdSetting";

            var result = await _httpService.GetFromFundingClaimAPIWithMSILAuthenticationAsync(uri);

            _logger.LogInformation(
                $"FundingClaimApiService successfully executed GetFeedReadWarningThresholdSetting for uri {uri}." + Environment.NewLine
                + $"Result is {result}.");

            return result;
        }

        /// <inheritdoc/>
        public async Task SendFeedExceededReadThresholdWarningEmail(FeedReadThresholdExceededWarningMessage message)
        {
            var uri = $"{_apiSettings.ReconciliationUri}SendFeedExceededReadThresholdWarningEmail";
            await _httpService.PostWithMSILAuthenticationAsync(uri, message);

            _logger.LogInformation(
                $"Creating azure service bus message to send email for feed read exceeding threshold. Bookmark[{message.BookmarkId}], Url[{message.LastPageUrl}]");
        }
    }
}