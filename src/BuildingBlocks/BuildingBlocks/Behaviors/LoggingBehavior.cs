using BuildingBlocks.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger=logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Handling {RequestType} - response={Response} - RequestData={RequestData} at {DateTime}",
                typeof(TRequest).Name, typeof(TResponse).Name,request, DateTime.UtcNow);
            var timer = new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeElapsed = timer.Elapsed;
            if(timeElapsed.Seconds > 3)
                this._logger.LogWarning("[PERFORMANCE] Long Running Request: {RequestType} ({TimeTaken}s)",
                    typeof(TRequest).Name, timeElapsed.Seconds);
            this._logger.LogInformation("Handled {RequestType} - response={Response}",
                typeof(TRequest).Name, typeof(TResponse).Name);
            return response;
        }
    }
}
