using System;
using MongoDB.Driver;

namespace Publisher.App.Persistence
{
    public sealed class SessionHolder : ISessionHolder
    {
        public IClientSessionHandle Session { get; private set; }

        public void SetSession(IClientSessionHandle clientSessionHandle)
            => this.Session = clientSessionHandle ?? throw new ArgumentNullException(nameof(clientSessionHandle));

        public void Dispose()
        {
            this.Session?.Dispose();
            this.Session = null;
        }
    }
}