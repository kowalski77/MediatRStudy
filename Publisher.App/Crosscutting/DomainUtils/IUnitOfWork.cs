using System;
using System.Threading.Tasks;

namespace Publisher.App.Crosscutting.DomainUtils
{
    public interface IUnitOfWork : IDisposable
    {
        bool TransactionInProgress { get; }

        Task StartAsync();

        Task CommitAsync();
    }
}