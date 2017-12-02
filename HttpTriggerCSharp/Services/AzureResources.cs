using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Insights;

namespace ARMNotify
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
                new RedisWatcher(),
                new PublicIPWatcher(),
                new ApplicationGatewayWatcher(),
                new VirtualNetworkGatewayWatcher()
            };
        }

        public static InsightsClient Client { get; internal set; }
    }
}
