namespace MicroTodo.IntegrationTests.Fakers;

using Bogus;

using MicroTodo.Domain.Entities;

public class TodoFaker : Faker<TodoItem>
{
    public TodoFaker()
    {
        RuleFor(x => x.Title, f => f.Lorem.Sentence());
        RuleFor(x => x.State, f => f.PickRandom<TodoItemState>());
        RuleFor(x => x.DueDate, f => f.Date.Future());
    }

    public TodoFaker WithGroupId(long groupId)
    {
        RuleFor(x => x.GroupId, _ => groupId);
        return this;
    }
}
