using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Publisher.App.Application.Infrastructure;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private static readonly TransactionOptions TransactionOptions =
            new TransactionOptions(ReadConcern.Snapshot, ReadPreference.Primary, WriteConcern.WMajority);

        private readonly IMongoDatabase mongoDatabase;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;
        private readonly ISessionHolder sessionHolder;

        public UnitOfWork(
            IMongoDatabase mongoDatabase, 
            ISessionHolder sessionHolder, 
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this.mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
            this.sessionHolder = sessionHolder ?? throw new ArgumentNullException(nameof(sessionHolder));
            this.domainEventsDispatcher = domainEventsDispatcher ?? throw new ArgumentNullException(nameof(domainEventsDispatcher));
        }

        public bool TransactionInProgress => this.sessionHolder.Session?.IsInTransaction ?? false;

        public async Task StartAsync()
        {
            var session = await this.mongoDatabase.Client.StartSessionAsync().ConfigureAwait(false);
            Console.WriteLine($"Start Session Id : {session.ServerSession.Id}");
            session.StartTransaction(TransactionOptions);
            this.sessionHolder.SetSession(session);
        }

        public async Task CommitAsync()
        {
            if (this.sessionHolder.Session == null || !this.TransactionInProgress)
            {
                throw new UoWException("Transaction must be started");
            }

            Console.WriteLine($"Commit Session Id : {this.sessionHolder.Session.ServerSession.Id}");

            await this.domainEventsDispatcher.DispatchEvents().ConfigureAwait(false);
            await this.sessionHolder.Session.CommitTransactionAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            this.sessionHolder.Dispose();
        }
    }
}