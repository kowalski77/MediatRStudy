using System;
using System.Collections.Generic;

namespace Publisher.App.Crosscutting.DomainUtils
{
    public abstract class Entity
    {
        private List<BaseEvent> domainEvents;

        public IReadOnlyList<BaseEvent> DomainEvents => this.domainEvents;

        public Guid Id { get; protected set; }

        protected Entity()
        {
        }

        protected Entity(Guid id) : this()
        {
            this.Id = id;
        }

        public void ClearDomainEvents()
        {
            this.domainEvents?.Clear();
        }

        protected void AddDomainEvent<T>(T eventItem)
            where T : BaseEvent
        {
            this.domainEvents ??= new List<BaseEvent>();
            this.domainEvents.Add(eventItem);
        }
    }
}