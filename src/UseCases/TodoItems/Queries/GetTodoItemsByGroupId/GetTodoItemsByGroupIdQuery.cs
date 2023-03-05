namespace MicroTodo.UseCases.TodoItems.Queries;

public record GetTodoItemsByGroupIdQuery : IRequest<IEnumerable<TodoItem>>
{
    public long GroupId { get; set; }
}
