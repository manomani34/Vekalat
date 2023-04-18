using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Foroosh.Application.Common
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

          

           var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                            .Select(x => x.Validate(context))
                            .SelectMany(x => x.Errors)
                            .Where(x => x != null)
                            .ToList();
            var reqCtxt = (request as IAPIRequestContext);
            if (failures.Any())
            {
                if (reqCtxt != null)
                {
                    if (reqCtxt.APIRequestMetadata != null)
                    {
                        var metadata = reqCtxt.APIRequestMetadata;
                        Log.Error("Error Info is: {failures}", failures);
                    }
                }
                throw new ValidationException(failures);
            }
            Log.Debug("Validation Is Successfull");
            return await next();
        }
    }
}
