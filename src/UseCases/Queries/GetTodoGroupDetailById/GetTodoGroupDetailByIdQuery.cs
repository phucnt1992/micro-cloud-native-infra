namespace MicroTodo.UseCases.Queries;

public record GetTodoGroupDetailByIdQuery : IRequest<TodoGroup>
{
    public long Id { get; set; }
}
