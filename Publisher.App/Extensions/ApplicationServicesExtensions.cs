using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Publisher.App.Application.CommandHandlers;
using Publisher.App.Application.DomainEventHandlers;
using Publisher.App.Application.Infrastructure;
using Publisher.App.Crosscutting.DomainUtils;
using Publisher.App.Domain.DomainEvents;

namespace Publisher.App
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ServiceFactory>(p => p.GetService);

            // TODO: move
            services.AddScoped<IChangeTracker, ChangeTracker>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddNotificationHandlerDecorator<ApplyDiscountNotification, ApplyDiscountNotificationHandler>();
            services.AddDomainEventHandlerDecorator<IngredientDiscountApplied, IngredientDiscountAppliedDomainEventHandler>();

            return services;
        }

        private static IServiceCollection AddNotificationHandlerDecorator<TNotification, TNotificationHandler>(this IServiceCollection services)
            where TNotification : INotification 
            where TNotificationHandler : class, INotificationHandler<TNotification>
        {
            services.AddScoped<TNotificationHandler>()
                .AddScoped<INotificationHandler<TNotification>>(provider =>
                    new RetryDecoratorNotificationHandler<TNotification>(
                        provider.GetService<TNotificationHandler>(),
                        provider.GetService<IUnitOfWork>(),
                        provider.GetService<ILogger<RetryDecoratorNotificationHandler<TNotification>>>()));

            return services;
        }

        private static IServiceCollection AddDomainEventHandlerDecorator<TEvent, TEventHandler>(this IServiceCollection services)
            where TEvent : BaseEvent
            where TEventHandler : class, IDomainEventNotificationHandler<TEvent>
        {
            services
                .AddScoped<TEventHandler>()
                .AddScoped<INotificationHandler<TEvent>>(provider =>
                    new DomainEventDecoratorNotificationHandler<TEvent>(
                        provider.GetService<TEventHandler>(),
                        provider.GetService<IDomainEventsDispatcher>(),
                        provider.GetService<ILogger<DomainEventDecoratorNotificationHandler<TEvent>>>()));

            return services;
        }
    }
}