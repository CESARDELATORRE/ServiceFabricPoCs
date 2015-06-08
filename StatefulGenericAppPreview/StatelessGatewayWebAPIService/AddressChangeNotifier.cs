

namespace StatelessGatewayWebAPIService
{
    using System;
    using System.Collections.Concurrent;
    using System.Fabric;
    using System.Fabric.Description;

    using Microsoft.ServiceFabric.Services;

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddressChangeNotifier<TPartitionKey> : IAddressChangeNotifier<TPartitionKey>
    {
        private long filterId;
        private readonly Uri serviceName;
        private readonly ServiceNotificationFilterDescription filterDescription;
        private readonly FabricClient fabricClient;
        private readonly ConcurrentDictionary<TPartitionKey, Guid> partitionMap;
        private readonly ConcurrentDictionary<Guid, string> addresses;

        public async Task<Tuple<string, Guid>> GetAddressAsync(TPartitionKey partitionKey, CancellationToken cancellationToken)
        {
            Guid partitionId;

            if (!this.partitionMap.TryGetValue(partitionKey, out partitionId))
            {
                ServicePartitionResolver spr = new ServicePartitionResolver(() => this.fabricClient);
                ResolvedServicePartition partition = await spr.ResolveAsync(this.serviceName, partitionKey, cancellationToken);

                partitionId = partition.Info.Id;
                this.partitionMap[partitionKey] = partitionId;
                this.addresses[partitionId] = partition.GetEndpoint().Address;
            }

            return new Tuple<string, Guid>(this.addresses[partitionId], partitionId);
        }

        public AddressChangeNotifier(string serviceName)
            : this(serviceName, true)
        {
        }

        public AddressChangeNotifier(string serviceName, bool matchPrimaryOnly)
        {
            this.partitionMap = new ConcurrentDictionary<TPartitionKey, Guid>();
            this.addresses = new ConcurrentDictionary<Guid, string>();

            this.serviceName = new Uri(serviceName);
            this.fabricClient = new FabricClient();
            this.filterDescription = new ServiceNotificationFilterDescription()
            {
                MatchPrimaryChangeOnly = matchPrimaryOnly,
                Name = this.serviceName
            };
        }

        public async Task StartUpdating()
        {
            this.filterId = await fabricClient.ServiceManager.RegisterServiceNotificationFilterAsync(this.filterDescription);
            fabricClient.ServiceManager.ServiceNotificationFilterMatched += (s, e) => this.OnNotification(e);
        }

        public async Task StopUpdating()
        {
            await fabricClient.ServiceManager.UnregisterServiceNotificationFilterAsync(this.filterId);
        }

        private void OnNotification(EventArgs e)
        {
            ServiceNotification notification = ((System.Fabric.FabricClient.ServiceManagementClient.ServiceNotificationEventArgs)e).Notification;
            
            ResolvedServiceEndpoint endpoint = notification.Endpoints.First(
                ep => ep.Role == ServiceEndpointRole.StatefulPrimary || ep.Role == ServiceEndpointRole.Stateless);

            this.addresses[notification.PartitionId] = endpoint.Address;
        }

    }


}
