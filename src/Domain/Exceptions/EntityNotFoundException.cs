using System.Diagnostics.CodeAnalysis;

namespace MicroTodo.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }

    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull<T>([NotNull] T? entity, object key) where T : class
    {
        if (entity is null)
        {
            throw new EntityNotFoundException(nameof(T), key);
        }
    }

    public static void ThrowIfFalse<T>(bool condition, object key)
    {
        if (!condition)
        {
            throw new EntityNotFoundException(nameof(T), key);
        }
    }
}
