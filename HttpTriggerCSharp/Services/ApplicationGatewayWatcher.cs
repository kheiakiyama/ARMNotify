using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.AppService.Fluent;

namespace AzureResourcesWasteNotice
{
    public class ApplicationGatewayWatcher : IAzureResourceWatcher
    {
        public ApplicationGatewayWatcher()
        {
        }

        public string RecommendPolicy => "Create ApplicationGateway when you need";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure
                .ApplicationGateways
                .List()
                .WithoutNoProduction()
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = $"{q.Sku.Name} - {q.Size.Value}",
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
