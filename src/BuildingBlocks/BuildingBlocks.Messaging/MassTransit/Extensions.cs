using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        // Make centralized configuration for MassTransit and RabbitMQ to be ruasable across all microservices
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(configs =>
            {
                configs.SetKebabCaseEndpointNameFormatter();

                if(assembly is not null)
                    configs.AddConsumers(assembly);

                configs.UsingRabbitMq((context, configrator) =>
                {
                    configrator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:Username"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });

                    configrator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
