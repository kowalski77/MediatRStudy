using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Publisher.App.Infrastructure;

namespace Publisher.App
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });

            services.AddMassTransitHostedService();
            services.AddScoped<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}