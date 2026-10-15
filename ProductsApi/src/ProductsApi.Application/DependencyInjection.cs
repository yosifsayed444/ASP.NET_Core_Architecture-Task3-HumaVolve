using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductsApi.Application.Abstractions;

namespace ProductsApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // سجّل كل الـ Handlers تلقائيًا (أي class بيطبّق ICommandHandler أو IQueryHandler)
        var handlerInterfaces = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

        foreach (var type in assembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false }))
        {
            foreach (var i in type.GetInterfaces()
                         .Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition())))
            {
                services.AddScoped(i, type);
            }
        }

        // سجّل كل الـ Validators
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }
}
