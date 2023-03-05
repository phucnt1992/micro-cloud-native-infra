namespace MicroTodo.UseCases.Queries;

public record GetAllTodoGroupsQuery : IRequest<IEnumerable<TodoGroup>>
{
}
