using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;
using MSILAuthenticationResult = Microsoft.Identity.Client.AuthenticationResult;

namespace Pds.FundingClaim.DataProcessor.Services.Implementations
{
    /// <inheritdoc cref="IAuthenticationService"/>
    public class AuthenticationService : IAuthenticationService
    {
        private FundingClaimApiSettings _msilAuthFundingClaimApiSettings;
        private FCSApiSettings _msilAuthFCSApiSettings;
        private DataCollectionApiSettings _msilAuthApiSettings;
        private ISystemProvider _systemProvider;

        private MSILAuthenticationResult _msilAuthDCApiAccessToken = null;
        private MSILAuthenticationResult _msilAuthFundingClaimApiAccessToken = null;
        private MSILAuthenticationResult _msilAuthFCSApiAccessToken = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="fundingClaimApiSettings">The funding claim api settings.</param>
        /// <param name="fcsApiSettings">The fcs api settings.</param>
        /// <param name="dataCollectionApiSettings">The data collection settings.</param>
        /// <param name="systemProvider">The system provider.</param>
        public AuthenticationService(
            IOptions<FundingClaimApiSettings> fundingClaimApiSettings,
            IOptions<FCSApiSettings> fcsApiSettings,
            IOptions<DataCollectionApiSettings> dataCollectionApiSettings,
            ISystemProvider systemProvider)
        {
            _msilAuthFundingClaimApiSettings = fundingClaimApiSettings.Value;
            _msilAuthFCSApiSettings = fcsApiSettings.Value;
            _msilAuthApiSettings = dataCollectionApiSettings.Value;
            _systemProvider = systemProvider;
        }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenForFundingClaimMSIL()
        {
            if (_msilAuthFundingClaimApiAccessToken == null || _msilAuthFundingClaimApiAccessToken.ExpiresOn <= _systemProvider.Now())
            {
                _msilAuthFundingClaimApiAccessToken = await RequestAccessTokenNoScopeWithMSIL(_msilAuthFundingClaimApiSettings).ConfigureAwait(false);
            }

            return _msilAuthFundingClaimApiAccessToken.AccessToken;
        }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenForFCSMSIL()
        {
            if (_msilAuthFCSApiAccessToken == null || _msilAuthFCSApiAccessToken.ExpiresOn <= _systemProvider.Now())
            {
                _msilAuthFCSApiAccessToken = await RequestAccessTokenNoScopeWithMSIL(_msilAuthFCSApiSettings).ConfigureAwait(false);
            }

            return _msilAuthFCSApiAccessToken.AccessToken;
        }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenForDCMSIL()
        {
            if (_msilAuthDCApiAccessToken == null || _msilAuthDCApiAccessToken.ExpiresOn <= _systemProvider.Now())
            {
                _msilAuthDCApiAccessToken = await RequestAccessTokenForDCWithMSIL().ConfigureAwait(false);
            }

            return _msilAuthDCApiAccessToken.AccessToken;
        }

        /// <summary>
        /// Request an access token that can be used with the Data collection funding claim Api using MSIL.
        /// </summary>
        /// <returns>An auth result and the token.</returns>
        private async Task<MSILAuthenticationResult> RequestAccessTokenForDCWithMSIL()
        {
            string[] scopes = { _msilAuthApiSettings.Scope };

            var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(_msilAuthApiSettings.ClientId)
                                                      .WithClientSecret(_msilAuthApiSettings.ClientSecret)
                                                      .WithAuthority(new Uri($"{_msilAuthApiSettings.Authority}{_msilAuthApiSettings.TenantId}"))
                                                      .Build();

            var token = await confidentialClientApplication.AcquireTokenForClient(scopes)
                              .ExecuteAsync();

            if (token == null)
            {
                throw new Exception("Access token cannot be acquired for MSIL Auth");
            }

            return token;
        }

        /// <summary>
        /// Request an access token that can be used where there is no scope set in the AppSettings.
        /// </summary>
        /// <returns>An auth result and the token.</returns>
        private async Task<MSILAuthenticationResult> RequestAccessTokenNoScopeWithMSIL(MSILAuthenticationSettings settings)
        {
            string[] scopes = { $"{settings.AppIdUri}/.default" };

            var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(settings.ClientId)
                                                      .WithClientSecret(settings.ClientSecret)
                                                      .WithAuthority(new Uri($"{settings.Authority}{settings.TenantId}"))
                                                      .Build();

            var token = await confidentialClientApplication.AcquireTokenForClient(scopes)
                              .ExecuteAsync();

            if (token == null)
            {
                throw new Exception("Access token cannot be acquired for MSIL Auth");
            }

            return token;
        }
    }
}