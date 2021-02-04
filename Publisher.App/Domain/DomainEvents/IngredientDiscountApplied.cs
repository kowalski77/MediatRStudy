using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Domain.DomainEvents
{
    public class IngredientDiscountApplied : BaseEvent
    {
        public IngredientDiscountApplied(
            decimal discount,
            Entity owner) : base(owner)
        {
            this.Discount = discount;
        }

        public decimal Discount { get; }
    }
}