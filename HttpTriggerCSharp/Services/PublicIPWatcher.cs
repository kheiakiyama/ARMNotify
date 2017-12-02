using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.AppService.Fluent;

namespace ARMNotify
{
    public class PublicIPWatcher : IAzureResourceWatcher
    {
        public PublicIPWatcher()
        {
        }

        public string RecommendPolicy => "Forget to remove not attached PublicIP";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure
                .PublicIPAddresses
                .List()
                .WithoutNoProduction()
                .Where(q => !q.HasAssignedLoadBalancer && !q.HasAssignedNetworkInterface)
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.Sku.Value,
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
