namespace MicroTodo.UseCases.Commands;

public record CreateTodoCommand : IRequest<long>
{
    public string Title { get; set; } = string.Empty;
    public TodoItemState State { get; set; } = TodoItemState.NotStarted;
    public DateTime? DueDate { get; set; }
    public long? GroupId { get; set; }
}
