
using Common.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddScoped<GlobalExceptionHandlerMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            return app;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }
    }
}
