namespace MicroTodo.Domain.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exists.")
    {
    }

    public EntityAlreadyExistsException() : base()
    {
    }

    public EntityAlreadyExistsException(string message) : base(message)
    {
    }

    public EntityAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
