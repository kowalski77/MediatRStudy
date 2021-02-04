using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Publisher.App.Application.Infrastructure
{
    public sealed class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IChangeTracker changeTracker;
        private readonly IMediator mediator;

        public DomainEventsDispatcher(IChangeTracker changeTracker, IMediator mediator)
        {
            this.changeTracker = changeTracker;
            this.mediator = mediator;
        }

        public async Task DispatchEvents()
        {
            var domainEntities = this.changeTracker.GetEntitiesTracked()
                .Where(x => x.DomainEvents != null && x.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();

            domainEntities.ToList().ForEach(x => x.ClearDomainEvents());

            var tasks = domainEvents.Select(async domainEvent =>
            {
                await this.mediator.Publish(domainEvent).ConfigureAwait(false);
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}