﻿using System;
using System.Linq;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace AzureResourcesWasteNotice
{
    class Program
    {
        static void Main(string[] args)
        {
			// Authenticate
			var credentials = SdkContext.AzureCredentialsFactory
				.FromFile(Environment.GetEnvironmentVariable("AZURE_AUTH_LOCATION"));

			var azure = Azure
				.Configure()
				.WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
				.Authenticate(credentials)
				.WithDefaultSubscription();

            var items = AzureResources.GetAzureWasteResources(azure);
            foreach (var item in items)
                Console.WriteLine($"{item.ResourceGroupName} - {item.ResourceTypeName}:{item.Name} - {item.State}");
            Console.WriteLine("Hello World!");
        }
    }
}
