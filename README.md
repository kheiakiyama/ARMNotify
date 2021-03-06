# ARMNotify
ARMNotify push notify Azure payed resources to WebHookUrl

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fkheiakiyama%2FARMNotify%2Fmaster%2Fazuredeploy.json" target="_blank">
  <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fkheiakiyama%2FARMNotify%2Fmaster%2Fazuredeploy.json" target="_blank">
  <img src="http://armviz.io/visualizebutton.png"/>
</a>

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

