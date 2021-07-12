using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ValidationGenerator
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            var failures = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(failure => !(failure is null))
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
