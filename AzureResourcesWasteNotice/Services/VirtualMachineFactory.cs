using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
namespace AzureResourcesWasteNotice
{
    public class VirtualMachineFactory : IAzureResourceFactory
    {
        public VirtualMachineFactory()
        {
        }

        public AzureResource[] Collect(IAzure azure)
        {
            return azure.VirtualMachines
                .List()
                .Where(q => q.PowerState != PowerState.Deallocated)
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.PowerState.ToString(),
                })
                .ToArray();
        }
    }
}
