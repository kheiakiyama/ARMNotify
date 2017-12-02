using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
namespace ARMNotify
{
    public class DiskWatcher : IAzureResourceWatcher
    {
        public DiskWatcher()
        {
        }

        public string RecommendPolicy => "Forget to remove not attached disk";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure.Disks
                .List()
                .WithoutNoProduction()
                .Where(q => !q.IsAttachedToVirtualMachine)
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.Sku.ToString(),
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
