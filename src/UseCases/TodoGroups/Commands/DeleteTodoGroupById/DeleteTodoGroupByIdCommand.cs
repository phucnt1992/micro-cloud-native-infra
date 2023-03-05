namespace MicroTodo.UseCases.TodoGroups.Commands;

public record DeleteTodoGroupByIdCommand : IRequest
{
    public long Id { get; set; }
}
