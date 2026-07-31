using System.Threading.Tasks;

namespace Pds.FundingClaim.DataProcessor.Services.Interfaces
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Get an access token to use for the funding claim Api using MSIL.
        /// </summary>
        /// <returns>An the authentication token.</returns>
        Task<string> GetAccessTokenForFundingClaimMSIL();

        /// <summary>
        /// Get an access token to use for the FCS Api using MSIL.
        /// </summary>
        /// <returns>An the authentication token.</returns>
        Task<string> GetAccessTokenForFCSMSIL();

        /// <summary>
        /// Get an access token to use for the data collection Api.
        /// </summary>
        /// <returns>An the authentication token.</returns>
        Task<string> GetAccessTokenForDCMSIL();
    }
}