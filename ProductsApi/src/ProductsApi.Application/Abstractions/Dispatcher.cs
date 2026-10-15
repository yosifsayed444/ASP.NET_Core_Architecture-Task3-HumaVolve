using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsApi.Application.Abstractions;

/// <summary>
/// بيدوّر على الـ Handler المناسب للـ Command/Query، وقبل ما ينفّذ بيشغّل الـ Validator (لو موجود).
/// (بديل بسيط لـ MediatR من غير أي package إضافي أو license.)
/// </summary>
public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        => Execute<TResponse>(typeof(ICommandHandler<,>), command, cancellationToken);

    public Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        => Execute<TResponse>(typeof(IQueryHandler<,>), query, cancellationToken);

    private async Task<TResponse> Execute<TResponse>(Type handlerInterface, object request, CancellationToken ct)
    {
        var requestType = request.GetType();

        // 1) Validation
        var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
        if (serviceProvider.GetService(validatorType) is IValidator validator)
        {
            var result = await validator.ValidateAsync(new ValidationContext<object>(request), ct);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        // 2) Handler
        var handlerType = handlerInterface.MakeGenericType(requestType, typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);
        var handleMethod = handlerType.GetMethod("Handle")!;

        return await (Task<TResponse>)handleMethod.Invoke(handler, [request, ct])!;
    }
}
