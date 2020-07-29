﻿using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class NetworkInterfaceDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.network/networkinterfaces" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));
            
            var resource = new NetworkInterfaceResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), resultRowEntry[3]?.ToString());
            return resource;
        }
    }
}