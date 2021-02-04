using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Publisher.App.Application.CommandHandlers;
using Publisher.App.Domain;

namespace Publisher.App
{
    internal static class Program
    {
        private static IServiceProvider serviceProvider;

        private static async Task Main()
        {
            await ConfigureServicesAsync().ConfigureAwait(false);
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            await SendNotifications().ConfigureAwait(false);

            DisposeServices();
        }
        private static async Task SendNotifications()
        {
            var testNotification = new ApplyDiscountNotification
            {
                RecipeName = "recipe1",
                IngredientType = IngredientType.AcidRegulator,
                Total = 10
            };

            var notifications = Enumerable.Repeat(testNotification, 10);
            await Task.WhenAll(notifications.Select(ProcessNotificationsAsync)).ConfigureAwait(false);
        }

        private static async Task ProcessNotificationsAsync(ApplyDiscountNotification notification)
        {
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(notification).ConfigureAwait(false);
        }

        private static async Task ConfigureServicesAsync()
        {
            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());

            services
                .AddMediator()
                .AddPersistence()
                .AddMassTransit();

            serviceProvider = services.BuildServiceProvider(true);
            await serviceProvider.SeedDatabaseAsync().ConfigureAwait(false);
        }

        private static void DisposeServices()
        {
            switch (serviceProvider)
            {
                case null:
                    return;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}