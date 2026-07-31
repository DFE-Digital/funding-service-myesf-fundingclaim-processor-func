using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pds.Core.Logging;
using Pds.Core.Telemetry.ApplicationInsights;
using Pds.FundingClaim.DataProcessor.Services.DependencyInjection;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddLoggerAdapter()
    .AddPdsApplicationInsightsTelemetry(options =>
    {
        builder.Configuration.Bind(nameof(PdsApplicationInsightsConfiguration), options);
        options.Component = typeof(Program).Assembly.GetName().Name;
    })
    .AddFundingClaimDataProcessorFeatureServices();

builder.Build().Run();