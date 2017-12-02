using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
namespace ARMNotify
{
    public class VirtualMachineWatcher : IAzureResourceWatcher
    {
        public VirtualMachineWatcher()
        {
        }

        public string RecommendPolicy => "Stopped VM is not charged";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure.VirtualMachines
                .List()
                .WithoutNoProduction()
                .Where(q => q.PowerState != PowerState.Deallocated)
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.Size.ToString(),
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
