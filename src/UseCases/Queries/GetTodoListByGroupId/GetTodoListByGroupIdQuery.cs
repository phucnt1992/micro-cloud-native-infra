namespace MicroTodo.UseCases.Queries;

public record GetTodoListByGroupIdQuery : IRequest<IEnumerable<TodoEntity>>
{
    public long GroupId { get; set; }
}
