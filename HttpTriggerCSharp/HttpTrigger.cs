﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Insights;

namespace ARMNotify
{
    public class HttpTrigger
    {
        public static async Task<IActionResult> Run(HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var credentialFile = Path.GetTempPath() + @"\.credentical.txt";
            await WriteCredentialFile(credentialFile, log);
            await AuthenticateInsights(credentialFile);
            // Authenticate
            var azure = Azure
                .Authenticate(credentialFile)
                .WithDefaultSubscription();
            var factories = AzureResources.GetFactories();
            var msg = "";
            var subscription = azure.GetCurrentSubscription();
            msg += $"### General\n";
            msg += $"SubscriptionId: {subscription.SubscriptionId}\n";
            msg += $"SubscriptionName: {subscription.DisplayName}\n";
            foreach (var factory in factories)
            {
                var items = factory.Collect(azure);
                if (items.Length > 0)
                {
                    msg += $"\n### {items[0].ResourceTypeName} - {factory.RecommendPolicy}\n";
                    foreach (var item in items)
                        msg += $"- {item.ResourceGroupName} - {item.Name} - {item.State} - {item.GetActivitiesBy()}\n";
                }
            }

            log.Info(msg);
            return new OkObjectResult(msg);
        }

        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        private static async Task WriteCredentialFile(string fileName, TraceWriter log)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                await sw.WriteLineAsync($"subscription={GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID")}");
                await sw.WriteLineAsync($"client={GetEnvironmentVariable("AZURE_CLIENT_ID")}");
                await sw.WriteLineAsync($"key={GetEnvironmentVariable("AZURE_SECRET")}");
                await sw.WriteLineAsync($"tenant={GetEnvironmentVariable("AZURE_TENANT")}");
            }
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

        private static async Task PostMessage(string url, string message)
        {
            var client = new HttpClient();
            var context = JsonConvert.SerializeObject(new
            {
                text = message
            });
            await client.PostAsync(url, new StringContent(context, Encoding.UTF8, "application/json"));
        }
    }
}
