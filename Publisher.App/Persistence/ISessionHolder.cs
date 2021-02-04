using System;
using MongoDB.Driver;

namespace Publisher.App.Persistence
{
    public interface ISessionHolder : IDisposable
    {
        IClientSessionHandle Session { get; }

        void SetSession(IClientSessionHandle clientSessionHandle);
    }
}