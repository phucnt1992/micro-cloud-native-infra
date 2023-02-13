namespace MicroTodo.UseCases.Queries;
public record GetTodoDetailByIdQuery : IRequest<TodoEntity>
{
    public long Id { get; set; }
}
