using System;
using System.Collections.Generic;
using System.Linq;
using Publisher.App.Crosscutting.DomainUtils;
using Publisher.App.Domain.DomainEvents;

namespace Publisher.App.Domain
{
    public class Recipe : Entity, IAggregateRoot
    {
        private List<Ingredient> ingredients = new List<Ingredient>();

        public Recipe(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; private set; }

        public IReadOnlyCollection<Ingredient> Ingredients => this.ingredients ??= new List<Ingredient>();

        public void AddIngredient(Ingredient ingredient)
        {
            this.ingredients.Add(ingredient);
        }

        public void ApplyDiscount(IngredientType ingredientType, decimal discount)
        {
            foreach (var ingredient in this.ingredients.Where(x => x.IngredientType == ingredientType))
            {
                ingredient.ApplyDiscount(discount);
            }

            this.AddDomainEvent(new IngredientDiscountApplied(discount, this));
        }
    }
}