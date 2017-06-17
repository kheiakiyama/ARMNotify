using System;
using System.Linq;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace AzureResourcesWasteNotice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Authenticate
            var azure = Azure
                .Authenticate(System.AppContext.BaseDirectory + @"\.credentical.txt")
                .WithDefaultSubscription();

            var items = AzureResources.GetAzureWasteResources(azure);
            var msg = "";
            var subscription = azure.GetCurrentSubscription();
            msg += $"SubscriptionId: {subscription.SubscriptionId}\n";
            msg += $"SubscriptionName: {subscription.DisplayName}\n";
            foreach (var item in items)
                msg += $"{item.ResourceGroupName} - {item.ResourceTypeName}:{item.Name} - {item.State}\n";
            var webhook_url = args[0];
            PostMessage(webhook_url, $"```{msg}```");
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
