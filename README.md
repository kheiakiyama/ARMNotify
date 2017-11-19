# AzureResourcesWasteNotice
AzureResourcesWasteNotice notify Azure payed resources to WebHookUrl

## Requirements
### Environment Variables

- `AZURE_SUBSCRIPTION_ID`  
Target subscription id
- `AZURE_TENANT`  
Azure Active Directory tenant id exist of Service pricipal
- `AZURE_CLIENT_ID`  
Service Principal App Id
- `AZURE_SECRET`  
Service Principal Password
- `WEBHOOK_URL`  
AzureResourcesWasteNotice notify to this URL.  
Tested webhook is [Slack Incoming Webhook](https://api.slack.com/incoming-webhooks) only.

## Debug

### Use azure-functions-cli

```
cd HttpTriggerCSharp/bin/Debug/netstandard2.0
func host start
```

### Use Visual Studio

change `HttpTriggerCoreDebug` to StartUpProject 

### Deploy to Azure Function

```
cd HttpTriggerCSharp/bin/Debug/netstandard2.0
func azure login
func azure functionapp publish (function app name)
```

### References
#### How to create Service Principal
- [Create Service Principal by azure-cli](https://docs.microsoft.com/en-us/cli/azure/ad/sp?view=azure-cli-latest#az_ad_sp_create_for_rbac) 

