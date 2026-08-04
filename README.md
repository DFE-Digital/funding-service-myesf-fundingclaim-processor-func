# Manage Your Education and Skills Funding FundingClaim DataProcessor Function
The Manage Your Education and Skills Funding FundingClaim DataProcessor Function app is used by the MYESF to allow the following:
- Get funding claims in the current window from Data Collections API and store them.
- Reads FCS reconciliation feed and store them.
- Autowithdraw the funding claims that have passed the signature close date.

## Provider

[The Department for Education](https://www.gov.uk/government/organisations/department-for-education)

## About this project

This project is a .Net 8 timer triggered Azure Function project utilizing an Azure Function App for deployment.

**Note:** The project is currently being updated to be containerised via Docker where the deployment method and target will change, this document will be updated when these changes have been finalised.

# Local Configuration Guide

For running the application locally, `local.settings.json` file need to be created in the `Pds.FundingClaim.DataProcessor.Func` project. Below, and included in the repo, there is `local.settings.example.json` which can be used as a base and populated with the required values, which can be retrieved from the Azure Portal.

## Application Settings (`local.settings.json`)
```json
{
  "IsEncrypted": false,
  "Values": {
    "AutowithdrawFundingClaimScheduleTriggerTime": "0 5,15,25,35,45,55 * * * *",
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "DataCollectionApiEndpointSettings:BaseUri": "",
    "DataCollectionApiEndpointSettings:GetFundingClaimCollectionsDetailsEndpoint": "",
    "DataCollectionApiEndpointSettings:GetFundingClaimsEndpoint": "",
    "DataCollectionApiSettings:Authority": "",
    "DataCollectionApiSettings:ClientId": "",
    "DataCollectionApiSettings:ClientSecret": "",
    "DataCollectionApiSettings:Scope": "",
    "DataCollectionApiSettings:TenantId": "",
    "Environment": "",
    "FCSApiEndpointSettings:BaseUri": "",
    "FCSApiSettings:AppIdUri": "",
    "FCSApiSettings:Authority": "",
    "FCSApiSettings:ClientId": "",
    "FCSApiSettings:ClientSecret": "",
    "FCSApiSettings:TenantId": "",
    "FUNCTIONS_EXTENSION_VERSION": "~4",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "FundingClaimApiSettings:AppIdUri": "",
    "FundingClaimApiSettings:Authority": "",
    "FundingClaimApiSettings:ClientId": "",
    "FundingClaimApiSettings:ClientSecret": "",
    "FundingClaimApiSettings:TenantId": "",
    "GetFundingClaimScheduleTriggerTime": "0 1,11,21,31,41,51 * * * *",
    "InternalApiSettings:BaseUri": "",
    "PdsApplicationInsights:Environment": "",
    "PdsApplicationInsights:InstrumentationKey": "",
    "ReconciliationFeedReaderScheduleTriggerTime": "0 7,17,27,37,47,57 * * * *",
    "UpdateFundingClaimWindowScheduleTriggerTime": "0 0,10,20,30,40,50 * * * *"
  }
}
```
### Setting Details

- **`AutowithdrawFundingClaimScheduleTriggerTime`**  
  The CRON expression determining how often the automatic withdrawal funding claim process triggers (configured for minutes ending in 5).

- **`AzureWebJobsStorage`**
  The core application setting used by the Azure Functions and Azure WebJobs runtime to establish a connection to an Azure Storage account.

- **`AzureWebJobsDashboard`**
  The core application setting used by the Azure Functions and Azure WebJobs runtime to establish a connection to an Azure Jobs dashboard.

- **`DataCollectionApiEndpointSettings:BaseUri`**  
  The primary base URL used to construct endpoints for communicating with the Data Collection API.

- **`DataCollectionApiEndpointSettings:GetFundingClaimCollectionsDetailsEndpoint`**  
  The specific endpoint path or route used to fetch detailed collection information for funding claims.

- **`DataCollectionApiEndpointSettings:GetFundingClaimsEndpoint`**  
  The specific endpoint path or route used to fetch general funding claim data from the Data Collection system.

- **`DataCollectionApiSettings:Authority`**  
  The identity provider URL (typically Azure AD) used to authenticate requests against the Data Collection API.

- **`DataCollectionApiSettings:ClientId`**  
  The unique application ID used to identify this application when authenticating with the Data Collection API.

- **`DataCollectionApiSettings:ClientSecret`**  
  The secret key credential used by this application to prove its identity to the Data Collection API token service.

- **`DataCollectionApiSettings:Scope`**  
  The permission scopes or resources requested during authentication for authorization against the Data Collection API.

- **`DataCollectionApiSettings:TenantId`**  
  The specific Azure Active Directory tenant ID hosting the Data Collection API registration.

- **`Environment`**  
  The runtime environment setting for the application deployment (e.g., Development, Staging, or Production).

- **`FCSApiEndpointSettings:BaseUri`**  
  The primary base URL used to construct communication endpoints for the Funding Calculation Service (FCS) API.

- **`FCSApiSettings:AppIdUri`**  
  The App ID URI (Application ID URI) used to identify the target FCS API resource during token generation.

- **`FCSApiSettings:Authority`**  
  The identity provider endpoint used to request authentication tokens for interacting with the FCS API.

- **`FCSApiSettings:ClientId`**  
  The application registration ID used to identify this client app when making requests to the FCS API.

- **`FCSApiSettings:ClientSecret`**  
  The secret key or credential used to authenticate this app's client identity against the FCS API identity provider.

- **`FCSApiSettings:TenantId`**  
  The specific Azure Active Directory tenant identifier governing the FCS API instance.

- **`FUNCTIONS_EXTENSION_VERSION`**  
  The target major version of the Azure Functions runtime environment host (set here to version ~4).

- **`FUNCTIONS_WORKER_RUNTIME`**  
  The isolated worker model process setting that dictates how the .NET code executes (configured for isolated process mode).

- **`FundingClaimApiSettings:AppIdUri`**  
  The explicit Application ID URI required to authenticate and authorize against the target Funding Claim API resource.

- **`FundingClaimApiSettings:Authority`**  
  The authentication token authority URL utilized when connecting to the secure Funding Claim API service.

- **`FundingClaimApiSettings:ClientId`**  
  The application identification registration code mapped to this software client for accessing the Funding Claim API.

- **`FundingClaimApiSettings:ClientSecret`**  
  The secure client secret credential assigned to this application for authorization verification by the Funding Claim API.

- **`FundingClaimApiSettings:TenantId`**  
  The target directory tenant code in Azure used during the token acquisition handshake for the Funding Claim API.

- **`GetFundingClaimScheduleTriggerTime`**  
  The CRON schedule layout setting the exact timing interval to query and fetch active funding claims (configured for minutes ending in 1).

- **`InternalApiSettings:BaseUri`**  
  The base network address utilized by the internal API routing system to process internal service calls.

- **`PdsApplicationInsights:Environment`**  
  The environment label configuration assigned specifically to telemetric data traces within the Pds Application Insights module.

- **`PdsApplicationInsights:InstrumentationKey`**  
  The specialized target key linking application event metrics directly into the Pds Application Insights dashboard.

- **`ReconciliationFeedReaderScheduleTriggerTime`**  
  The CRON execution schedule rule mapping out how frequently the system reads incoming data from the reconciliation feed (configured for minutes ending in 7).

- **`UpdateFundingClaimWindowScheduleTriggerTime`**  
  The CRON layout parameter defining the processing schedule that executes system checks to refresh funding claim visibility windows (configured for minutes ending in 0).

## Build and Test

To build and test locally, you can either use Visual Studio, Visual Studio Code or simply use dotnet CLI `dotnet build` and `dotnet test` more information in dotnet CLI can be found at <https://docs.microsoft.com/en-us/dotnet/core/tools/>.

## Contribute

To contribute,

- If you are part of the team then create a branch for changes and then submit your changes for review by creating a pull request.
- If you are external to the organisation then fork this repository and make necessary changes and then submit your changes for review by creating a pull request.