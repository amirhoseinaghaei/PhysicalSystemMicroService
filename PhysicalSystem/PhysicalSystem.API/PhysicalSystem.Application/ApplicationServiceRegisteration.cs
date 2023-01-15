using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhysicalSystem.Application.Business;
using PhysicalSystem.Application.Utils.Kafka;
using PhysicalSystem.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalSystem.Application
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection ConfigureApplicationServicces(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<MainSimulator>();
            return services
            .AddSingleton<IEnvironmentConfig, EnvironmentConfig>()
            .AddSingleton<IKafkaSender, KafkaSender>();


        }
    }
}
