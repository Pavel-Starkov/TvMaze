using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using Scraper.Interfaces;

namespace Scraper.Actor
{
    internal class SchedulingActorService : ActorService
    {
        public SchedulingActorService(StatefulServiceContext context, ActorTypeInformation typeInfo)
            : base(context, typeInfo)
        { }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await base.RunAsync(cancellationToken);

            var lowKey = ((Int64RangePartitionInformation) Partition.PartitionInfo).LowKey;
            
            var proxy = ActorProxy.Create<IScraper>(new ActorId(lowKey));

            await proxy.StartWork();
        }
    }
}
