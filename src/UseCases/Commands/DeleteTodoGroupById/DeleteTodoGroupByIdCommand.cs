namespace MicroTodo.UseCases.Commands;

public record DeleteTodoGroupByIdCommand : IRequest
{
    public long Id { get; set; }
}
