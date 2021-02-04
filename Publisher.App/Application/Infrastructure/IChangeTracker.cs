using System.Collections.Generic;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Application.Infrastructure
{
    public interface IChangeTracker
    {
        void UpdateEntity(Entity entity);

        IEnumerable<Entity> GetEntitiesTracked();

        void ClearEntities();
    }
}