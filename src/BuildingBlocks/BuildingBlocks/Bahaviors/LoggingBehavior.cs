

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Bahaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest :notnull,  IRequest<TResponse> where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation($"[START] Handle request={typeof(TRequest).Name}- Response{typeof(TResponse).Name}- Request Data {request}");
            var timer = new Stopwatch();
            timer.Start();

            var response = await next();
            timer.Stop();
            var timetaken = timer.Elapsed;
            if (timetaken.Seconds > 3)
            {
                logger.LogWarning($"[PERFORMANCE] The request={typeof(TRequest).Name}- took {timetaken.Seconds}");
            }
            logger.LogWarning($"[END] Handled {typeof(TRequest).Name} with {typeof(TResponse).Name}");
            return response;
        }
    }
}
