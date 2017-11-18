using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
namespace AzureResourcesWasteNotice
{
    public class DatabaseWatcher : IAzureResourceWatcher
    {
        public DatabaseWatcher()
        {
        }

        public AzureResource[] Collect(IAzure azure)
        {
            return azure.SqlServers
                .List()
                .WithoutNoProduction()
                .SelectMany(q => q.Databases.List())
                .Where(q => q.Edition != "" && q.Name != "master")
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.Edition,
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
