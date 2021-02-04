using System.Threading.Tasks;

namespace Publisher.App.Infrastructure
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message)
            where T : class;
    }
}