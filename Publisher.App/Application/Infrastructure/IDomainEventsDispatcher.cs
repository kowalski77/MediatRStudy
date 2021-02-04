using System.Threading.Tasks;

namespace Publisher.App.Application.Infrastructure
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEvents();
    }
}