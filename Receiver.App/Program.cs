using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Receiver.App
{
    internal static class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection();
            services.AddScoped<ITestService, TestService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<StudyConsumer>();

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
            });

            var provider = services.BuildServiceProvider();

            var busControl = provider.GetRequiredService<IBusControl>();

            await busControl.StartAsync();

            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(Console.ReadLine);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}