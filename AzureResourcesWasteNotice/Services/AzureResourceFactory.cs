using System;
using System.Collections.Generic;
using Microsoft.Azure.Management.Fluent;

namespace AzureResourcesWasteNotice
{
    public interface IAzureResourceFactory
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
}
