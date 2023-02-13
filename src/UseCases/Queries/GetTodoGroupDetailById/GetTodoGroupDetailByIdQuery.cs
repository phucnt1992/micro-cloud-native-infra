namespace MicroTodo.UseCases.Queries;

public record GetTodoGroupDetailByIdQuery : IRequest<TodoGroupEntity>
{
    public long Id { get; set; }
}
