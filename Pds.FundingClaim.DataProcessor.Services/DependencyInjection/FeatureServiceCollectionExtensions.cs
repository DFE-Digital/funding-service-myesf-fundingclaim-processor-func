using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pds.FundingClaim.DataProcessor.Services.Configurations;
using Pds.FundingClaim.DataProcessor.Services.Implementations;
using Pds.FundingClaim.DataProcessor.Services.Interfaces;

namespace Pds.FundingClaim.DataProcessor.Services.DependencyInjection
{
    /// <summary>
    /// Extensions class for <see cref="IServiceCollection"/> for registering the feature's services.
    /// </summary>
    public static class FeatureServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the FundingClaim Data Processor feature to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the feature's services to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddFundingClaimDataProcessorFeatureServices(this IServiceCollection services)
        {
            services.AddOptions<InternalApiSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(InternalApiSettings)).Bind(settings);
                    });

            services.AddOptions<FundingClaimApiSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(FundingClaimApiSettings)).Bind(settings);
                    });

            services.AddOptions<FCSApiSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(FCSApiSettings)).Bind(settings);
                    });

            services.AddOptions<FCSApiEndpointSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(FCSApiEndpointSettings)).Bind(settings);
                    });

            services.AddOptions<DataCollectionApiSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(DataCollectionApiSettings)).Bind(settings);
                    });

            services.AddOptions<DataCollectionApiEndpointSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(DataCollectionApiEndpointSettings)).Bind(settings);
                    });

            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            services.AddHttpClient<IHttpService, HttpService>().ConfigurePrimaryHttpMessageHandler(service =>
            {
                var serviceConfig = service.GetService<IOptions<DataCollectionApiSettings>>().Value;
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                return clientHandler;
            });
            services.AddSingleton<ISystemProvider, SystemProvider>();

            services.AddSingleton<IFundingClaimDataService, FundingClaimDataService>();

            services.AddSingleton<IFundingClaimApiService, FundingClaimApiService>();

            services.AddSingleton<IFundingClaimService, FundingClaimService>();

            services.AddSingleton<IFeedProcessor, GuidItemIdBasedAtomFeedProcessor>();

            services.AddSingleton<IReconciliationService, ReconciliationService>();

            services.AddSingleton<IReconciliationDataService, ReconciliationDataService>();

            return services;
        }
    }
}