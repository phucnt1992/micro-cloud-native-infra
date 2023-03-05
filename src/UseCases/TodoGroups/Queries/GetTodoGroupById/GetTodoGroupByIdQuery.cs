namespace MicroTodo.UseCases.TodoGroups.Queries;

public record GetTodoGroupByIdQuery : IRequest<TodoGroup>
{
    public long Id { get; set; }
}
