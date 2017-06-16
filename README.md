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

### How to run

```
dotnet AzureResourcesWasteNotice.dll {SLACK_WEBHOOK_URL}
```

