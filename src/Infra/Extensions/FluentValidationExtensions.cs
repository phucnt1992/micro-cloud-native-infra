using FluentValidation;

namespace MicroTodo.Infra.Extensions;

public static class FluentValidationExtensions
{
    public static IDictionary<string, string[]> ToDictionary(this ValidationException exception)
    {
        return exception.Errors
          .GroupBy(x => x.PropertyName)
          .ToDictionary(
            g => g.Key,
            g => g.Select(x => x.ErrorMessage).ToArray()
          );
    }
}
