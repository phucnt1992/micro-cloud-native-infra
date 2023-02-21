using System.Diagnostics;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MicroTodo.Infra.Pipelines;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogDebug("Handling {TRequestName}: {@Request}", requestName, request);
        var response = await next();
        _logger.LogDebug("Handled {TRequestName}: {@Response}", requestName, response);

        return response;
    }
}
