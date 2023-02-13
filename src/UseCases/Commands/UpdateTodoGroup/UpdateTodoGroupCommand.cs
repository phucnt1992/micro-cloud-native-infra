namespace MicroTodo.UseCases.Commands;

public record UpdateTodoGroupCommand : IRequest<TodoGroupEntity>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
