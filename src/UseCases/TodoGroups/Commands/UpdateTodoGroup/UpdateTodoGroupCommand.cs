namespace MicroTodo.UseCases.TodoGroups.Commands;

public record UpdateTodoGroupCommand : IRequest<TodoGroup>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
