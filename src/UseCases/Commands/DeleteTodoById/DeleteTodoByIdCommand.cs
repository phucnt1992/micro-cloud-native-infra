namespace MicroTodo.UseCases.Commands;

public record DeleteTodoByIdCommand : IRequest
{
    public long Id { get; set; }
}
