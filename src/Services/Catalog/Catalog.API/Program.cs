using BuildingBlocks.Bahaviors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//add services to container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database"));
}).UseLightweightSessions();

var app = builder.Build();
//add http request pipeline

app.MapCarter();
app.UseExceptionHandler(
    exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(
            async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception ==null)
                    return;

                var problem = new ProblemDetails
                {
                    Title = exception.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.StackTrace
                };
                var logger=context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation(exception, exception.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                context.Response.WriteAsJsonAsync(problem);
            }
            );
    }
    );
app.Run();
