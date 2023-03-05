namespace MicroTodo.Domain.Entities;
public class TodoItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public TodoItemState State { get; set; } = TodoItemState.NotStarted;
    public DateTime? DueDate { get; set; }

    public long? GroupId { get; set; }
    public TodoGroup? BelongToGroup { get; set; }
}
