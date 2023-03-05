namespace MicroTodo.IntegrationTests.Fakers;

using Bogus;

using MicroTodo.Domain.Entities;

public class TodoGroupFaker : Faker<TodoGroup>
{
    public TodoGroupFaker()
    {
        RuleFor(x => x.Name, f => f.Lorem.Sentence());
    }

    public TodoGroupFaker WithId(long id)
    {
        RuleFor(x => x.Id, _ => id);
        return this;
    }
}
