using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Rest.Azure.Authentication;
using System.Threading.Tasks;
using Microsoft.Azure.Insights;
using System.IO;
using System.Collections.Generic;

namespace AzureResourcesWasteNotice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var credentialFile = System.AppContext.BaseDirectory + @"\.credentical.txt";
            await AuthenticateInsights(credentialFile);
            // Authenticate
            var azure = Azure
                .Authenticate(credentialFile)
                .WithDefaultSubscription();
            var items = AzureResources.GetAzureWasteResources(azure);
            var msg = "";
            var subscription = azure.GetCurrentSubscription();
            msg += $"SubscriptionId: {subscription.SubscriptionId}\n";
            msg += $"SubscriptionName: {subscription.DisplayName}\n";
            foreach (var item in items)
                msg += $"{item.ResourceGroupName} - {item.ResourceTypeName}:{item.Name} - {item.GetActivitiesBy()} - {item.State}\n";
#if DEBUG
            Console.WriteLine(msg);
            Console.ReadLine();
#else
            var webhook_url = args[0];
            PostMessage(webhook_url, $"```{msg}```");
#endif
        }

        private static async Task AuthenticateInsights(string credentialFile)
        {
            using (var fileReader = new StreamReader(new FileStream(credentialFile, FileMode.Open)))
            {
                var lines = await fileReader.ReadToEndAsync();
                var dic = new Dictionary<string, string>();
                foreach (var line in lines.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var pair = line.Split(new string[] { "=" }, StringSplitOptions.None);
                    dic.Add(pair[0].ToLower(), pair[1]);
                }
                var credentials = await ApplicationTokenProvider.LoginSilentAsync(dic["tenant"], dic["client"], dic["key"]);
                AzureResources.Client = new InsightsClient(credentials);
                AzureResources.Client.SubscriptionId = dic["subscription"];
            }
        }

        private static void PostMessage(string url, string message)
        {
            var client = new HttpClient();
            var context = JsonConvert.SerializeObject(new
            {
                text = message
            });
            var task = client.PostAsync(url, new StringContent(context, Encoding.UTF8, "application/json"));
            task.Wait();
        }
    }
}
