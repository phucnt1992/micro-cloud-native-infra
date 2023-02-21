using MediatR;

using Microsoft.EntityFrameworkCore;

using MicroTodo.Infra.Persistence;
using MicroTodo.Infra.Pipelines;
using MicroTodo.MinimalApi.Endpoints;
using MicroTodo.UseCases;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlite("Data Source=MicroTodo.db"));

builder.Services.AddTransient<IApplicationDbContext>(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

builder.Services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService("MicroTodo.MinimalApi"))
    .WithTracing(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSqlClientInstrumentation()
    );

builder.Services.AddMediatR(typeof(Anchor))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapGroup("/api/todo-groups")
    .MapTodoGroupsEndpoints();

app.MapGroup("/api/todo")
    .MapTodoEndpoints();

app.Run();

public partial class Program
{ }
