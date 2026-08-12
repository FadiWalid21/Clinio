using Clinio.Application.Common.Behaviors;
using Clinio.Application.Features.Auth.Commands.Login;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Clinio.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}