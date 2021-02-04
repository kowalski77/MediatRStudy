namespace Publisher.App.Crosscutting.DomainUtils
{
    public abstract class BaseEvent : IDomainEvent
    {
        protected BaseEvent(Entity owner)
        {
            this.Owner = owner;
        }

        public Entity Owner { get; }
    }
}