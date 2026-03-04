using ErrorOr;
using FluentValidation;
using MediatR;

namespace Echoes.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct
        )
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                return (dynamic)
                    failures.Select(f => Error.Validation(f.PropertyName, f.ErrorMessage)).ToList();
            }

            return await next(ct);
        }
    }
}
