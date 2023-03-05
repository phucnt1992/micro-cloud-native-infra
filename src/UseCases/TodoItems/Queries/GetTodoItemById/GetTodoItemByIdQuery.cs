namespace MicroTodo.UseCases.TodoItems.Queries;

public record GetTodoItemByIdQuery : IRequest<TodoItem>
{
    public long Id { get; set; }
}
