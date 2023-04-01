using System.Text.Json;
using Broadcast.Infra.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Broadcast.Service
{
    public static class Bootstrap
    {
        public static IServiceCollection AddBootstrap(this IServiceCollection services, IConfiguration configuration = null)
        {
            //if (configuration is null)
            //{
            //    LoadDefaultConfiguration(services);
            //}
            //else
            //{
            //    LoadConfiguration(services, configuration);
            //}

            LoadDefaultConfiguration(services);

            return services;
        }

        private static void LoadConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            
        }

        private static void LoadDefaultConfiguration(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            using (StreamReader r = new StreamReader("../Broadcast.Service/appsettigns.json"))
            {
                string json = r.ReadToEnd();
                appSettings = JsonSerializer.Deserialize<AppSettings>(json);
            }

            services.AddSingleton<IBroadcastRabbitMQ>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(appSettings?.Configurations?.UserName))
                {
                    factory.UserName = appSettings.Configurations.UserName;
                }

                if (!string.IsNullOrEmpty(appSettings?.Configurations?.Password))
                {
                    factory.Password = appSettings.Configurations.Password;
                }

                if (!string.IsNullOrEmpty(appSettings?.Configurations?.HostName))
                {
                    factory.HostName = appSettings.Configurations.HostName;
                }

                if (!string.IsNullOrEmpty(appSettings?.Configurations?.Port))
                {
                    factory.Port = Int32.Parse(appSettings.Configurations.Port);
                }

                return new BroadcastRabbitMQ(factory);
            });
        }
    }
}