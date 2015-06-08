using System;
using System.Threading;
using System.Threading.Tasks;
namespace StatelessGatewayWebAPIService
{
    interface IAddressChangeNotifier<TPartitionKey>
    {
        Task<Tuple<string, Guid>> GetAddressAsync(TPartitionKey partitionKey, CancellationToken cancellationToken);
        Task StartUpdating();
        Task StopUpdating();
    }
}
