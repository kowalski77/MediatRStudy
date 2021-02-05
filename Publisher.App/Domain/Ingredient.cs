using System;
using Publisher.App.Crosscutting.DomainUtils;

namespace Publisher.App.Domain
{
    public class Ingredient : Entity
    {
        public Ingredient(string name, decimal cost, IngredientType ingredientType)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            if (cost <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cost));
            }
            this.Cost = cost;
            this.IngredientType = ingredientType;
        }

        public string Name { get; private set; }

        public int DiscountsApplied { get; private set; }

        public IngredientType IngredientType { get; private set; }

        public decimal Cost { get; private set; }

        public void ApplyDiscount(decimal discount)
        {
            if (discount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(discount), "Discount to apply must be greater than zero.");
            }

            this.Cost -= (discount / 100);
            this.DiscountsApplied += 1;
        }
    }
}