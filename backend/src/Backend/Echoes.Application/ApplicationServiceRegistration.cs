using Echoes.Application.Core.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Echoes.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

        return services;
    }

    public static IServiceCollection AddSimulationServices(this IServiceCollection services)
    {
        return services;
    }
}
