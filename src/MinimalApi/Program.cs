using MediatR;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;
using MicroTodo.Infra.Pipelines;
using MicroTodo.MinimalApi.Endpoints;
using MicroTodo.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlite("Data Source=MicroTodo.db"));

builder.Services.AddScoped<IApplicationDbContext>(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

builder.Services.AddMediatR(typeof(Anchor))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.MapGroup("/api/todo-groups")
    .MapTodoGroupsEndpoints();

app.MapGroup("/api/todo")
    .MapTodoEndpoints();

app.Run();

public partial class Program
{ }
