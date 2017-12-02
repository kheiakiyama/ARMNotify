using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.AppService.Fluent;

namespace ARMNotify
{
    public class RedisWatcher : IAzureResourceWatcher
    {
        public RedisWatcher()
        {
        }

        public string RecommendPolicy => "Create Redis when you need";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure
                .RedisCaches
                .List()
                .WithoutNoProduction()
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.Sku.Name,
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
