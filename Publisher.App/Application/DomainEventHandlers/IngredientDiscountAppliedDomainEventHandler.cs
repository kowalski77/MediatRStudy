using System;
using System.Threading.Tasks;
using Contracts;
using Microsoft.Extensions.Logging;
using Publisher.App.Crosscutting.DomainUtils;
using Publisher.App.Domain.DomainEvents;
using Publisher.App.Infrastructure;

namespace Publisher.App.Application.DomainEventHandlers
{
    public sealed class IngredientDiscountAppliedDomainEventHandler : BaseDomainEventHandler<IngredientDiscountApplied>
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly ILogger<IngredientDiscountAppliedDomainEventHandler> logger;

        public IngredientDiscountAppliedDomainEventHandler(
            ILogger<IngredientDiscountAppliedDomainEventHandler> logger, 
            IMessagePublisher messagePublisher) : base(logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }

        protected override Task Handle(IngredientDiscountApplied domainEvent, Entity entity)
        {
            this.logger.LogInformation($"{nameof(IngredientDiscountApplied)} event received! Discount for ingredient with id: {domainEvent.Owner.Id} of {domainEvent.Discount}");

            this.messagePublisher.PublishAsync(new ValueEntered(domainEvent.Owner.Id.ToString()));

            return Task.CompletedTask;
        }
    }
}