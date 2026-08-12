using Clinio.Api.Middleware;
using Clinio.Application.Features.Auth.Commands.Login;
using FluentValidation;

namespace Clinio.Api;

public static class ApiServiceRegistration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddProblemDetails();

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        services.AddValidatorsFromAssembly(typeof(LoginCommand).Assembly);

        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supported = new[] { "en", "ar" };
            options.SetDefaultCulture("en")
                .AddSupportedCultures(supported)
                .AddSupportedUICultures(supported);
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
        
        // 1. Add CORS policy in services
        services.AddCors(options =>
        {
            options.AddPolicy("DevPolicy", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:4200",   // clinio-website
                        "https://localhost:4201"    // clinio-dashboard (if different port)
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // only if you use cookies/auth headers
            });
        });
        

        return services;
    }
}