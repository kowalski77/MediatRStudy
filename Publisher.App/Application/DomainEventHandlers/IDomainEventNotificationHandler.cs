using MediatR;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Application.DomainEventHandlers
{
    public interface IDomainEventNotificationHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : IDomainEvent
    {
    }
}