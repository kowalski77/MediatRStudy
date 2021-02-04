using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Publisher.App.Domain;

namespace Publisher.App.Application.CommandHandlers
{
    public class ApplyDiscountNotificationHandler : INotificationHandler<ApplyDiscountNotification>
    {
        private readonly ILogger<ApplyDiscountNotificationHandler> logger;
        private readonly IRecipeRepository recipeRepository;

        public ApplyDiscountNotificationHandler(
            ILogger<ApplyDiscountNotificationHandler> logger,
            IRecipeRepository recipeRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
        }

        public async Task Handle(ApplyDiscountNotification notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            this.logger.LogInformation($"{nameof(ApplyDiscountNotificationHandler)} called, " +
                                       $"message: Recipe {notification.RecipeName} Ingredient type {notification.IngredientType}, Going to apply discount of {notification.Total}");

            var recipe = await this.recipeRepository.GetRecipeAsync(notification.RecipeName).ConfigureAwait(false);

            recipe.ApplyDiscount(notification.IngredientType, notification.Total);

            await this.recipeRepository.SaveAsync(recipe).ConfigureAwait(false);

            this.logger.LogInformation($"message: Recipe {notification.RecipeName} Ingredient type {notification.IngredientType}, applied discount of {notification.Total}");
        }
    }
}