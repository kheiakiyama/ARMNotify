using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.AppService.Fluent;

namespace AzureResourcesWasteNotice
{
    public class AppServiceWatcher : IAzureResourceWatcher
    {
        public AppServiceWatcher()
        {
        }

        public string RecommendPolicy => "Free Sku is not charged";

        public AzureResource[] Collect(IAzure azure)
        {
            return azure.AppServices
                .AppServicePlans
                .List()
                .WithoutNoProduction()
                .Where(q => q.PricingTier.SkuDescription.Name != PricingTier.FreeF1.SkuDescription.Name)
                .Select(q => new AzureResource()
                {
                    ResourceGroupName = q.ResourceGroupName,
                    Name = q.Name,
	                ResourceTypeName = q.Type,
	                State = q.PricingTier.SkuDescription.Name,
                    ResourceId = q.Id,
                })
                .ToArray();
        }
    }
}
