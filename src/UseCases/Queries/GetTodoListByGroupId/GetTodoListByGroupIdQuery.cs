namespace MicroTodo.UseCases.Queries;

public record GetTodoListByGroupIdQuery : IRequest<IEnumerable<TodoItem>>
{
    public long GroupId { get; set; }
}
