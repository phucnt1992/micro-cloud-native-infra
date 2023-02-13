namespace MicroTodo.UseCases.Commands;

public record UpdateTodoCommand : IRequest<TodoEntity>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TodoState State { get; set; } = TodoState.NotStarted;
    public DateTime? DueDate { get; set; }
    public long? GroupId { get; set; }
}
