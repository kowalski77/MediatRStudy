using MediatR;

namespace Publisher.App.Crosscutting.DomainUtils
{
    public interface IDomainEvent : INotification
    {
    }
}