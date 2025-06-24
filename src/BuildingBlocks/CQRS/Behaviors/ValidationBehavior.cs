public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = validators
            .Select(async v => await v.ValidateAsync(context, cancellationToken))
            .SelectMany(result => result.Result.Errors)
            .Where(f => f != null)
            .Distinct()
            .ToArray();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return next();
    }
}
