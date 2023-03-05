using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MicroTodo.Infra.Pipelines;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(validators);
        ArgumentNullException.ThrowIfNull(logger);

        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            _logger.LogDebug("Validating {TRequestName} with {@TValidator}", typeof(TRequest).Name, _validators);

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task
                .WhenAll(_validators
                    .Select(v => v
                        .ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(x => x.Errors!.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                _logger.LogDebug("Validation failed for {TRequestName} with failures: {@Failures}", typeof(TRequest).Name, failures);
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
