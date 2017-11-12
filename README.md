# AzureResourcesWasteNotice
AzureResourcesWasteNotice notify Azure payed resources to Slack

## Getting Started

### Authenticating with Azure
AzureResourcesWasteNotice use Service Principal Credentials.

Please make `.credentical.txt`

```
subscription={AZURE_SUBSCRIPTION_ID}
client={AZURE_CLIENT_ID}
key={AZURE_SECRET}
tenant={AZURE_TENANT}
```

### Build

```
cd HttpTriggerCSharp
dotnet build
dotnet publish
```

### Debug

```
cd HttpTriggerCSharp/bin/Debug/netstandard2.0
func host start
```

### Deploy to Azure Function

```
cd HttpTriggerCSharp/bin/Debug/netstandard2.0
func azure login
func azure functionapp publish (function app name)
```