using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizSvc.Application.Configurations.Mappers;

namespace QuizSvc.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        // Register Automapper
        services.AddAutoMapper(typeof(AutoMapperProfile));

        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        return services;
    }
}
