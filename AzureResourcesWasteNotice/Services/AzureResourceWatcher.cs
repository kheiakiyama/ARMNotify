using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace AzureResourcesWasteNotice
{
    public interface IAzureResourceWatcher
    {
        AzureResource[] Collect(IAzure azure);
    }

    public class AzureResource
	{
        public string ResourceGroupName { get; set; }
		public string Name { get; set; }
		public string ResourceTypeName { get; set; }
		public string State { get; set; }
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
