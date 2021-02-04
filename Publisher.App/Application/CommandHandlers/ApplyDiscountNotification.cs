using MediatR;
using Publisher.App.Domain;

namespace Publisher.App.Application.CommandHandlers
{
    public class ApplyDiscountNotification : INotification
    {
        public string RecipeName { get; set; }

        public IngredientType IngredientType { get; set; }

        public decimal Total { get; set; }
    }
}