namespace MicroTodo.UseCases.TodoItems.Commands;

public record DeleteTodoItemByIdCommand : IRequest
{
    public long Id { get; set; }
}
