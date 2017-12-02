using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Rest.Azure.OData;
using Microsoft.Azure.Insights.Models;
using Microsoft.Azure.Insights;

namespace ARMNotify
{
    public interface IAzureResourceWatcher
    {
        AzureResource[] Collect(IAzure azure);
        string RecommendPolicy { get; }
    }

    public class AzureResource
    {
        public string ResourceGroupName { get; set; }
        public string Name { get; set; }
        public string ResourceTypeName { get; set; }
        public string State { get; set; }
        public string ResourceId { get; set; }

        public string GetActivitiesBy()
        {
            var filter = new ODataQuery<EventData>(string.Format("{0} and {1} and {2}",
                string.Format("eventTimestamp ge {0}", DateTime.UtcNow.AddDays(-89).ToString("o")),
                string.Format("eventTimestamp le {0}", DateTime.UtcNow.ToString("o")),
                string.Format("resourceUri eq {0}", ResourceId)
            ));
            var res = AzureResources.Client.Events.List(filter)
                .Where(q => q.Caller != null)
                .Select(q => q.Caller)
                .GroupBy(q => q)
                .Select(q => new { Name = q.FirstOrDefault(), Count = q.Count() })
                .ToArray();
            return res.Length > 0 ?
                string.Join(", ", res.Select(q => $"{q.Name}: {q.Count}").ToArray()) :
                "Unknown";
        }
    }

    public static class AzureResourceExtentions
    {
        private static readonly string PRODUCTION_TAG = "production";

        public static IEnumerable<T> WithoutNoProduction<T>(this IEnumerable<T> items) where T : IResource
        {
            return items.Where(q => !q.Tags.ContainsKey(PRODUCTION_TAG) || q.Tags[PRODUCTION_TAG].ToLower() != bool.TrueString.ToLower());
        }
    }
}
