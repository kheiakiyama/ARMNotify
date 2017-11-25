using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Insights;

namespace AzureResourcesWasteNotice
{
    public class AzureResources
    {
        public static IAzureResourceWatcher[] GetFactories()
        {
            return new IAzureResourceWatcher[] {
                new VirtualMachineWatcher(),
                new AppServiceWatcher(),
                new DatabaseWatcher(),
                new DiskWatcher(),
                new ApplicationGatewayWatcher(),
                new RedisWatcher()
            };
        }

        public static InsightsClient Client { get; internal set; }
    }
}
