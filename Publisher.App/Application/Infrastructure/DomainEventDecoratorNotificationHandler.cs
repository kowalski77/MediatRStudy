using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Publisher.App.Application.DomainEventHandlers;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Application.Infrastructure
{
    public sealed class DomainEventDecoratorNotificationHandler<TEvent> : IDomainEventNotificationHandler<TEvent>
        where TEvent : BaseEvent
    {
        private readonly INotificationHandler<TEvent> intercepted;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;
        private readonly ILogger<DomainEventDecoratorNotificationHandler<TEvent>> logger;

        public DomainEventDecoratorNotificationHandler(
            INotificationHandler<TEvent> intercepted, 
            IDomainEventsDispatcher domainEventsDispatcher, 
            ILogger<DomainEventDecoratorNotificationHandler<TEvent>> logger)
        {
            this.intercepted = intercepted ?? throw new ArgumentNullException(nameof(intercepted));
            this.domainEventsDispatcher = domainEventsDispatcher ?? throw new ArgumentNullException(nameof(domainEventsDispatcher));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(DomainEventDecoratorNotificationHandler<TEvent>)} activated");

            await this.intercepted.Handle(notification, cancellationToken).ConfigureAwait(false);
            await this.domainEventsDispatcher.DispatchEvents().ConfigureAwait(false);

            this.logger.LogInformation($"{nameof(DomainEventDecoratorNotificationHandler<TEvent>)} finished");
        }
    }
}