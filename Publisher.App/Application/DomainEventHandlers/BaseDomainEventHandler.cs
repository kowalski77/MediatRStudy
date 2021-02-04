using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Application.DomainEventHandlers
{
    public abstract class BaseDomainEventHandler<TEvent> : IDomainEventNotificationHandler<TEvent>
        where TEvent : BaseEvent
    {

        protected BaseDomainEventHandler(ILogger<BaseDomainEventHandler<TEvent>> logger)
        {
            this.Logger = logger;
        }

        public ILogger<BaseDomainEventHandler<TEvent>> Logger { get; }

        protected virtual bool CanHandle(object eventToHandle, Entity entity)
            => eventToHandle != null && typeof(TEvent) == eventToHandle.GetType();

        public Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            this.Logger.LogInformation(FormattableString.Invariant($"{typeof(TEvent).Name} event fired: {Environment.NewLine} {notification} for {notification.Owner.Id}"));

            return !this.CanHandle(notification, notification.Owner) ? 
                Task.CompletedTask : 
                this.Handle(notification, notification.Owner);
        }

        protected abstract Task Handle(TEvent domainEvent, Entity entity);
    }
}