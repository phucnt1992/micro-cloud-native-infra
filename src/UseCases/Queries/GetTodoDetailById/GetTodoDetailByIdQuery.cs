namespace MicroTodo.UseCases.Queries;
public record GetTodoDetailByIdQuery : IRequest<TodoItem>
{
    public long Id { get; set; }
}
