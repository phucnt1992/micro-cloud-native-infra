namespace MicroTodo.Domain.Entities;
public class TodoEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public TodoState State { get; set; } = TodoState.NotStarted;
    public DateTime? DueDate { get; set; }

    public long? GroupId { get; set; }
    public TodoGroupEntity? BelongToGroup { get; set; }

}
