namespace MicroTodo.UseCases.TodoGroups.Queries;

public record GetTodoGroupsQuery : IRequest<IEnumerable<TodoGroup>>
{
}
