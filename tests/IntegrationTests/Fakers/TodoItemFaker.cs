namespace MicroTodo.IntegrationTests.Fakers;

using Bogus;

using MicroTodo.Domain.Entities;

public class TodoItemFaker : Faker<TodoItem>
{
    public TodoItemFaker()
    {
        RuleFor(x => x.Title, f => f.Lorem.Sentence());
        RuleFor(x => x.State, f => f.PickRandom<TodoItemState>());
    }

    public TodoItemFaker WithDueDate()
    {
        RuleFor(x => x.DueDate, f => f.Date.Future().ToUniversalTime());
        return this;
    }

    public TodoItemFaker WithGroupId(long groupId)
    {
        RuleFor(x => x.GroupId, _ => groupId);
        return this;
    }

    public TodoItemFaker WithId(long id)
    {
        RuleFor(x => x.Id, _ => id);
        return this;
    }
}
