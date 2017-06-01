using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;

namespace AzureResourcesWasteNotice
{
    public class AzureResources
    {
        public static AzureResource[] GetAzureWasteResources(IAzure azure)
        {
            return GetFactories()
                .SelectMany(q => q.Collect(azure))
                .ToArray();
        }

        private static IAzureResourceFactory[] GetFactories()
        {
            return new IAzureResourceFactory[] {
                new VirtualMachineWatcher(),
            };
        }
    }
}
