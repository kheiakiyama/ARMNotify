using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
namespace AzureResourcesWasteNotice
{
    public class DiskWatcher : IAzureResourceWatcher
    {
        public DiskWatcher()
        {
        }

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
