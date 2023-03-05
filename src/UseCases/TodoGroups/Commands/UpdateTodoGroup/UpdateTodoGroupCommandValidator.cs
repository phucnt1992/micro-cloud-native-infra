namespace MicroTodo.UseCases.TodoGroups.Commands;

using FluentValidation;

public class UpdateTodoGroupCommandValidator : AbstractValidator<UpdateTodoGroupCommand>
{
    public UpdateTodoGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
