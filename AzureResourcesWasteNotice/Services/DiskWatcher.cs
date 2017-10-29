using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;

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
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = $"{q.Sku.AccountType.ToString()} , {q.SizeInGB}GB",
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
