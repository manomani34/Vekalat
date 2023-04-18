using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Foroosh.Application.Common
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            var timer = new Stopwatch();
            timer.Start();
            Log.Debug("Entering LoggingBehavior with request {Name}", typeof(TRequest).FullName);

            var reqCtxt = (request as IAPIRequestContext);
            if (reqCtxt != null)
            {
                if (reqCtxt.APIRequestMetadata != null)
                {
                    var metadata = reqCtxt.APIRequestMetadata;
                    Log.Information("Request Id: {RequestId}, Current User: {User},Feature: {Feature} Controller: {Controller}, Method: {Method}, Client Address: {ClientIp}", metadata.RequestId, metadata.CurrentUser, metadata.Feature, metadata.Controller, metadata.Method, metadata.ClientIp);
                    if (metadata.Parameters != null)
                    {
                        foreach (var param in metadata.Parameters)
                        {
                            Log.Debug("Parameter is: {ParameterName}: {ParameterValue}", param.Key, param.Value);
                        }
                    }
                }
            }

            var response = await next();

            timer.Stop();
            var ms = timer.ElapsedMilliseconds;
            if (ms >= 800)
            {
                Log.Warning("Elapsed Time is {ms}", ms);
            }
            else
            {
                Log.Debug("Elapsed Time is {ms}", ms);
            }
            Log.Debug("Leaving LoggingBehavior with request {Name}", typeof(TRequest).Name);

            return response;
        }
    }
}
