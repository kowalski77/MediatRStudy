using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Application.Infrastructure
{
    public sealed class ChangeTracker : IChangeTracker
    {
        private readonly ConcurrentDictionary<Guid, Entity> entitiesTracked = new ConcurrentDictionary<Guid, Entity>();

        public void UpdateEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.entitiesTracked.TryAdd(entity.Id, entity);
        }

        public IEnumerable<Entity> GetEntitiesTracked() => this.entitiesTracked.Values;

        public void ClearEntities() => this.entitiesTracked.Clear();
    }
}