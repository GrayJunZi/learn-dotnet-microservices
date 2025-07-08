using FluentValidation;
using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Pipelines;

public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest, IValidateSelf
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task
                .WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            if (!validationResults.Any(x => x.IsValid))
            {
                var errorMessages = validationResults.SelectMany(x => x.Errors)
                    .Where(x => x != null)
                    .Select(x => x.ErrorMessage)?.ToList() ?? [];

                return (TResponse)await ResponseWrapper.FailAsync(errorMessages);
            }
        }
        return await next();
    }
}


public interface IValidateSelf
{

}