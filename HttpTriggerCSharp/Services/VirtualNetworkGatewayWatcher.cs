using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.AppService.Fluent;

namespace ARMNotify
{
    public class VirtualNetworkGatewayWatcher : IAzureResourceWatcher
    {
        public VirtualNetworkGatewayWatcher()
        {
        }

        public string RecommendPolicy => "Create VirtualNetworkGateway when you need";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure
                .VirtualNetworkGateways
                .List()
                .WithoutNoProduction()
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = $"{q.GatewayType.Value} - {q.VpnType.Value} - {q.Sku.Name}",
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
