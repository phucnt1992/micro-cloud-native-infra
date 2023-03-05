namespace MicroTodo.UseCases.TodoItems.Commands;

using FluentValidation;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}
